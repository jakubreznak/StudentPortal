using System.Collections.Generic;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Http;
using API.Interfaces;
using API.DTOs;
using System.IO;
using System;
using System.Security.Claims;

namespace API.Controllers
{
    public class PredmetyController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly IFileService _fileService;
        public PredmetyController(DataContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Predmet>>> GetPredmety()
        {
            return await _context.Predmets.ToListAsync();
        }

        [HttpGet]
        [Route("getbyid/{id}")]
        [Authorize]
        public async Task<ActionResult<Predmet>> GetPredmet(int id)
        {
            return await _context.Predmets.Include("Files").FirstOrDefaultAsync(x => x.ID == id);
        }

        [HttpGet]
        [Route("getbyobor/{idObor}")]
        public async Task<ActionResult<IEnumerable<Predmet>>> GetPredmetyByObor(int idObor)
        {
            return await _context.Predmets.Where(p => p.oborIdNum == idObor).OrderByDescending(p => p.doporucenyRocnik.HasValue)
            .ThenBy(p => p.doporucenyRocnik).ThenBy(p => p.statut).ToListAsync();
        }

        [HttpPost("add-file/{predmetId}/{nazevMaterial}")]
        [Authorize]
        public async Task<ActionResult<List<Soubor>>> AddFile(List<IFormFile> files, int predmetId, string nazevMaterial)
        {
            if (files == null || files.Count == 0 || nazevMaterial.Length <= 0)
                return BadRequest("Nebyl vybrán žádný soubor nebo nebyl zadán název.");

            var predmet = await _context.Predmets.Include("Files").FirstOrDefaultAsync(x => x.ID == predmetId);
            if (predmet == null)
                return BadRequest();

            if(predmet.Files.Any(f => f.FileName == nazevMaterial))
                return BadRequest("Studijní materiál s tímto názvem již u tohoto předmětu existuje.");

            string cloudinaryFileName = predmet.nazev + " - " + nazevMaterial;

            if (files.Count == 1)
            {
                if (files[0].Length > 30000000)
                    return BadRequest("Příliš velký soubor. (max. 30MB)");

                var result = await _fileService.AddFileAsync(files[0], "", cloudinaryFileName);
                if (result.Error != null) return BadRequest();

                string extension = Path.GetExtension(files[0].FileName).Substring(1).ToUpper();

                var soubor = new Soubor
                {
                    Url = result.SecureUrl.AbsoluteUri,
                    PublicID = result.PublicId,
                    FileName = nazevMaterial,
                    Extension = extension,
                    DateAdded = DateTime.Now.ToString("dd'.'MM'.'yyyy"),
                    studentName = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                };

                predmet.Files.Add(soubor);
                if (await _context.SaveChangesAsync() > 0)
                {
                    return predmet.Files;
                }
                return BadRequest();
            }
            else if (files.Count > 1 && files.Count <= 50)
            {
                List<string> filesPublicID = new List<string>();
                string tag = predmetId.ToString() + '|' + nazevMaterial;

                long size = files.Sum(f => f.Length);
                if (size > 30000000)
                    return BadRequest("Soubory mohou mít dohromady maximálně 30 MB.");

                foreach (var file in files)
                {
                    var result = await _fileService.AddFileAsync(file, tag);
                    filesPublicID.Add(result.PublicId);
                    if (result.Error != null) return BadRequest();
                }

                var archiveResult = await _fileService.GenerateArchiveURLAsync(tag, cloudinaryFileName);

                foreach(string publicID in filesPublicID)
                {
                    await _fileService.RemoveFileAsync(publicID);
                }

                var soubor = new Soubor
                {
                    Url = archiveResult.SecureUrl,
                    PublicID = archiveResult.PublicId,
                    FileName = nazevMaterial,
                    Extension = "ZIP",
                    DateAdded = DateTime.Now.ToString("dd'.'MM'.'yyyy"),
                    studentName = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                };

                predmet.Files.Add(soubor);
                if (await _context.SaveChangesAsync() > 0)
                    return predmet.Files;
            }
            return BadRequest();
        }

        [Authorize]
        [HttpDelete("{predmetID}/{souborID}")]
        public async Task<ActionResult<Soubor>> DeleteMaterial(int predmetID, int souborID)
        {
            var predmet = await _context.Predmets.Include("Files").FirstOrDefaultAsync(p => p.ID == predmetID);
            var soubor = predmet.Files.FirstOrDefault(s => s.ID == souborID);
            predmet.Files.Remove(soubor);
            if (await _context.SaveChangesAsync() <= 0) return BadRequest();

            var result = await _fileService.RemoveFileAsync(soubor.PublicID);
            if (result.Error != null) return BadRequest();

            return soubor;
        }

    }
}