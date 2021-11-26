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
using API.HelpClass;
using API.Extensions;
using Microsoft.AspNetCore.Identity;

namespace API.Controllers
{
    public class PredmetyController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly IFileService _fileService;
        private readonly UserManager<Student> _userManager;
        public PredmetyController(DataContext context, IFileService fileService, UserManager<Student> userManager)
        {
            _context = context;
            _fileService = fileService;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Predmet>>> GetPredmety()
        {
            return await _context.Predmets.ToListAsync();
        }

        [HttpGet]
        [Route("getbyid/{id}")]
        [Authorize]
        public ActionResult<IEnumerable<Soubor>> GetMaterialy(int id, [FromQuery] MaterialParameters materialParameters)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var student = _userManager.Users.FirstOrDefault(s => s.UserName == username);
           
            var soubory = _context.Soubor.Where(x => x.PredmetID == id).ToList();
            int allItemsCount = soubory.Count();

            if(!string.IsNullOrEmpty(materialParameters.Nazev))
            {
                soubory = soubory.Where(x => x.FileName.ToLower().Contains(materialParameters.Nazev.ToLower())).ToList();
            }
            if(!string.IsNullOrEmpty(materialParameters.Typ))
            {
                soubory = soubory.Where(x => x.Extension.ToLower().Contains(materialParameters.Typ.ToLower())).ToList();
            }

            foreach(var soubor in soubory)
            {
                soubor.Predmet = null;
                soubor.StudentsLikedBy = _context.SouborLikes.Where(x => x.SouborId == soubor.ID).ToList();
                soubor.StudentsLikedBy.ForEach(x => x.Student = null);
            }

            switch (materialParameters.OrderBy)
            {
                case "datum":
                    soubory = soubory.OrderByDescending(x => x.ID).ToList();
                    break;
                case "nazev":
                    soubory = soubory.OrderBy(x => x.FileName).ToList();
                    break;
                case "likes":
                    soubory = soubory.OrderByDescending(x => x.StudentsLikedBy.Count()).ToList();
                    break;
            }

            var pagedFiles = PagedList<Soubor>.CreateFromList(soubory, materialParameters.PageNumber, materialParameters.PageSize);

            Response.AddPaginationHeader(pagedFiles.CurrentPage, pagedFiles.PageSize, pagedFiles.TotalCount, pagedFiles.TotalPages, allItemsCount);
            return Ok(pagedFiles);

        }

        [HttpGet]
        [Route("getname/{id}")]
        [Authorize]
        public async Task<ActionResult<Predmet>> GetPredmetName(int id)
        {
            return await _context.Predmets.FirstOrDefaultAsync(x => x.ID == id);
        }

        [HttpGet]
        [Route("student")]
        public ActionResult<IEnumerable<Predmet>> GetPredmetyStudenta([FromQuery] PredmetParams predmetParams)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var student = _userManager.Users.Include("predmetyStudenta").FirstOrDefault(s => s.UserName == username);

            var predmety = student.predmetyStudenta.OrderByDescending(p => p.doporucenyRocnik.HasValue)
             .ThenBy(p => p.doporucenyRocnik).ThenBy(p => p.statut).ToList();

            foreach(var predmet in predmety)
            {
                predmet.Students = null;
            }

            if(!String.IsNullOrEmpty(predmetParams.Nazev))
            {
                var words = predmetParams.Nazev.Split(' ');
                List<Predmet> predmets = new List<Predmet>();
                foreach(var word in words)
                {
                    predmets.AddRange(predmety.Where(x => x.nazev.ToLower().Contains(word.ToLower())));
                }
                predmety = predmets.Distinct().ToList();
            }

            if(predmetParams.Statut != "all" && predmetParams.Statut != "bez")
            {
                predmety = predmety.Where(x => x.statut == predmetParams.Statut).ToList();
            }

            if(predmetParams.Statut == "bez")
            {
                predmety = predmety.Where(x => string.IsNullOrEmpty(x.statut)).ToList();
            }            

            if(predmetParams.Rocnik == 0 || predmetParams.Rocnik > 4)
            {
                return predmety;
            }
            else if(predmetParams.Rocnik == 4)
            {
                return Ok(predmety.Where(x => x.doporucenyRocnik == null));
            }
            return Ok(predmety.Where(x => x.doporucenyRocnik == predmetParams.Rocnik));
        }

        [HttpGet]
        [Route("student/count")]
        [Authorize]
        public ActionResult<int> GetPredmetyStudentaCount()
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var student = _userManager.Users.Include("predmetyStudenta").FirstOrDefault(s => s.UserName == username);

            var predmety = student.predmetyStudenta.OrderByDescending(p => p.doporucenyRocnik.HasValue)
             .ThenBy(p => p.doporucenyRocnik).ThenBy(p => p.statut).ToList();
            return predmety.Count();
        }

        [HttpPost("add-file/{predmetId}/{nazevMaterial}")]
        [Authorize]
        public async Task<ActionResult<List<Soubor>>> AddFile(List<IFormFile> files, int predmetId, string nazevMaterial)
        {
            if (files == null || files.Count == 0 || nazevMaterial.Length <= 0)
                return BadRequest("Nebyl vybrán žádný soubor nebo nebyl zadán název.");

            if (nazevMaterial.Length > 200)
                return BadRequest("Název materiálu může mít maximálně 200 znaků.");

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

                var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var student = _userManager.Users.FirstOrDefault(s => s.UserName == username);

                var soubor = new Soubor
                {
                    Url = result.SecureUrl.AbsoluteUri,
                    PublicID = result.PublicId,
                    FileName = nazevMaterial,
                    Extension = extension,
                    DateAdded = DateTime.Now.ToString("dd'.'MM'.'yyyy"),
                    studentName = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                    accountName = student.accountName
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
            var soubor = _context.Soubor.FirstOrDefault(s => s.ID == souborID);

            if(soubor.studentName != User.FindFirst(ClaimTypes.NameIdentifier)?.Value)
                return BadRequest("Nemáte oprávnění smazat tento studijní materiál.");

            _context.Soubor.Remove(soubor);
            if (await _context.SaveChangesAsync() <= 0) return BadRequest();

            var result = await _fileService.RemoveFileAsync(soubor.PublicID);
            if (result.Error != null) return BadRequest();

            return soubor;
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPut]
        public async Task<ActionResult> UpdatePredmetyUpol([FromBody] PredmetInfo[] predmety)
        {
            var predmets = predmety.GroupBy(p => new { p.zkratka, p.katedra}).Select(g => g.FirstOrDefault());

            foreach (var predmet in predmets)
            {
                if (!(await _context.Predmets.AnyAsync(p => p.zkratka == predmet.zkratka && p.katedra == predmet.katedra && p.statut == null && p.doporucenyRocnik == null)))
                {
                    var newPredmet = new Predmet();
                    newPredmet.katedra = predmet.katedra;
                    newPredmet.zkratka = predmet.zkratka;
                    newPredmet.nazev = predmet.nazev;
                    newPredmet.doporucenySemestr = predmet.semestr;
                    newPredmet.vyukaLS = predmet.vyukaLS;
                    newPredmet.vyukaZS = predmet.vyukaZS;

                    await _context.Predmets.AddAsync(newPredmet);                    
                }
            }
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet]
        [Route("all")]
        public async Task<ActionResult<IEnumerable<Predmet[]>>> GetPredmetyUniverzity([FromQuery] PredmetInfoParams predmetParams)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var student = _userManager.Users.Include("predmetyStudenta").FirstOrDefault(s => s.UserName == username);
            var predmetyStudenta = student.predmetyStudenta.ToList();

            var predmety = await _context.Predmets.Where(p => p.doporucenyRocnik == null && p.statut == null).OrderBy(p => p.nazev).ToListAsync();
            predmety = predmety.Where(p => !predmetyStudenta.Any(x => x.katedra == p.katedra && x.zkratka == p.zkratka)).ToList();
            int allItemsCount = predmety.Count();
            if(!String.IsNullOrEmpty(predmetParams.Nazev))
            {
                predmety = predmety.Where(x => x.nazev.ToLower().Contains(predmetParams.Nazev.Trim().ToLower())).ToList();
            }

            if(!String.IsNullOrEmpty(predmetParams.Katedra))
            {
                predmety = predmety.Where(x => x.katedra.ToLower().Contains(predmetParams.Katedra.Trim().ToLower())).ToList();
            }

            if(!String.IsNullOrEmpty(predmetParams.Zkratka))
            {
                predmety = predmety.Where(x => x.zkratka.ToLower().Contains(predmetParams.Zkratka.Trim().ToLower())).ToList();
            }

            var pagedPredmety = PagedList<Predmet>.CreateFromList(predmety, predmetParams.PageNumber, predmetParams.PageSize);

            Response.AddPaginationHeader(pagedPredmety.CurrentPage, pagedPredmety.PageSize, pagedPredmety.TotalCount, pagedPredmety.TotalPages, allItemsCount);
            return Ok(pagedPredmety);
        }

        [Authorize]
        [HttpPut("add")]
        public async Task<ActionResult> UpdatePredmetyUpol(Predmet predmet)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var student = _userManager.Users.Include("predmetyStudenta").FirstOrDefault(s => s.UserName == username);

            if(!student.predmetyStudenta.Contains(predmet))
            {
                student.predmetyStudenta.Add(predmet);
            }

            await _userManager.UpdateAsync(student);

            return Ok();
        }

        [Authorize]
        [HttpDelete("remove/{predmetID}")]
        public async Task<ActionResult<Student>> RemovePredmet(int predmetID)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var student = _userManager.Users.Include(u => u.predmetyStudenta).FirstOrDefault(s => s.UserName == username);

            var predmet = await _context.Predmets.Include(p => p.Students).FirstOrDefaultAsync(p => p.ID == predmetID);

            student.predmetyStudenta.RemoveAll(x => x.ID == predmet.ID);
            
            predmet.Students.RemoveAll(s => s.Id == student.Id);

            var result = await _userManager.UpdateAsync(student);
            if (!result.Succeeded) return BadRequest(result.Errors);

            _context.SaveChanges();

            return student;
        }

    }
}