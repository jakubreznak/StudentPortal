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
            return await  _context.Predmets.Include("Files").FirstOrDefaultAsync(x => x.ID == id);
        }

        [HttpGet]
        [Route("getbyobor/{idObor}")]
        public async Task<ActionResult<IEnumerable<Predmet>>> GetPredmetyByObor(int idObor)
        {
            return await _context.Predmets.Where(p => p.oborIdNum == idObor).OrderByDescending(p => p.doporucenyRocnik.HasValue)
            .ThenBy(p => p.doporucenyRocnik).ThenBy(p => p.statut).ToListAsync();
        }

        [HttpPost("add-file/{predmetId}")]
        [Authorize]
        public async Task<ActionResult<List<Soubor>>> AddFile(IFormFile file, int predmetId)
        {
            if(file.Length > 30000000)
                return BadRequest("Příliš velký soubor. (max. 30MB)");

            var predmet = await _context.Predmets.Include("Files").FirstOrDefaultAsync(x => x.ID == predmetId);
            if(predmet == null || file.Length > 30000000)
                return BadRequest();

            var result = await _fileService.AddFileAsync(file);

            string fileName = file.FileName;
            string extension = Path.GetExtension(fileName).Substring(1).ToUpper();
            fileName = fileName.Substring(0, fileName.Length - extension.Length);

            if (result.Error != null) return BadRequest();

            var soubor = new Soubor
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicID = result.PublicId,
                FileName = fileName,
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

    }
}