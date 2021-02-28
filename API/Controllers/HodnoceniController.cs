using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class HodnoceniController : BaseApiController
    {
        private readonly DataContext _context;
        public HodnoceniController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("{idPredmet}")]
        [Authorize]
        public async Task<ActionResult<List<Hodnoceni>>> GetPredmet(int idPredmet)
        {
            var predmet = await  _context.Predmets.Include("Hodnocenis").FirstOrDefaultAsync(x => x.ID == idPredmet);
            return predmet.Hodnocenis;
        }

        [HttpPost("{idPredmet}/{cislo}")]
        [Authorize]
        public async Task<ActionResult<List<Hodnoceni>>> PostRating(int idPredmet, int cislo, [FromBody] string text)
        {
            if(cislo < 1 || cislo > 10) return BadRequest();

            var predmet = await  _context.Predmets.Include("Hodnocenis").FirstOrDefaultAsync(x => x.ID == idPredmet);

            var hodnoceni = new Hodnoceni
            {
                text = text,
                rating = cislo,
                created = DateTime.Now.ToString("dd'.'MM'.'yyyy")
            };

            predmet.Hodnocenis.Add(hodnoceni);

            if(await _context.SaveChangesAsync() > 0)
            {
                return predmet.Hodnocenis;
            }
            return BadRequest();
        }
    }
}