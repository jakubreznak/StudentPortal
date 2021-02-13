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
        public ActionResult<Predmet> GetPredmet(int id)
        {
            return  _context.Predmets.Include("Files").FirstOrDefault(x => x.ID == id);
        }

        [HttpGet]
        [Route("getbyobor/{idObor}")]
        public async Task<ActionResult<IEnumerable<Predmet>>> GetPredmetyByObor(int idObor)
        {
            return await _context.Predmets.Where(p => p.oborIdNum == idObor).ToListAsync();
        }

        [HttpPost("add-file/{predmetId}")]
        public async Task<ActionResult<SouborDTO>> AddFile(IFormFile file, int predmetId)
        {
            var predmet = await _context.Predmets.FindAsync(predmetId);

            var result = await _fileService.AddFileAsync(file);

            string fileName = file.FileName;
            string extension = Path.GetExtension(fileName);
            fileName = fileName.Substring(0, fileName.Length - extension.Length);

            if (result.Error != null) return BadRequest();

            var soubor = new Soubor
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicID = result.PublicId,
                FileName = fileName,
                Extension = extension,
                DateAdded = DateTime.Now.ToString("dd'.'MM'.'yyyy")
            };

            predmet.Files.Add(soubor);
            if (await _context.SaveChangesAsync() > 0)
            {
                SouborDTO souborDto = new SouborDTO
                {
                    Url = soubor.Url,
                    PublicID = soubor.PublicID
                };

                return souborDto;
            }
            return BadRequest();

        }
    }
}