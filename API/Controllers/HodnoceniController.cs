using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using API.Extensions;
using API.HelpClass;
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
        public ActionResult<List<Hodnoceni>> GetPredmet(int idPredmet, [FromQuery] HodnoceniParams hodnoceniParams)
        {
            var predmet = _context.Predmets.FirstOrDefault(p => p.ID == idPredmet);
            var predmety = _context.Predmets.Include(p => p.Hodnocenis).Where(p => p.katedra == predmet.katedra && p.zkratka == predmet.zkratka);
            List<Hodnoceni> hodnoceni = new List<Hodnoceni>(); 
            foreach(var pred in predmety)
            {
                hodnoceni.AddRange(pred.Hodnocenis);
            }
            foreach(var hod in hodnoceni)
            {
                hod.predmet = null;
                hod.StudentsLikedBy = _context.HodnoceniLikes.Where(x => x.HodnoceniId == hod.ID).ToList();
                hod.StudentsLikedBy.ForEach(x => x.Student = null);
            }
            int allItemsCount = hodnoceni.Count(); 
            var pagedHodnoceni = PagedList<Hodnoceni>.CreateFromList(hodnoceni.OrderByDescending(x => x.ID), hodnoceniParams.PageNumber, hodnoceniParams.PageSize);

            Response.AddPaginationHeader(pagedHodnoceni.CurrentPage, pagedHodnoceni.PageSize, pagedHodnoceni.TotalCount, pagedHodnoceni.TotalPages, allItemsCount);
            return Ok(pagedHodnoceni);
        }

        [HttpGet("cislo/{idPredmet}")]
        [Authorize]
        public ActionResult<int> GetRatingNumber(int idPredmet)
        {
            var hodnoceni = _context.Predmets.Include("Hodnocenis").FirstOrDefault(x => x.ID == idPredmet).Hodnocenis;
            int ratingNumber = 0;
            foreach(var hod in hodnoceni)
            {
                ratingNumber += hod.rating;
            }

            return Ok(Math.Ceiling((double) ratingNumber / hodnoceni.Count));
        }

        [HttpPost("{idPredmet}/{cislo}")]
        [Authorize]
        public async Task<ActionResult<List<Hodnoceni>>> PostRating(int idPredmet, int cislo, [FromBody] string text)
        {
            if(cislo < 1 || cislo > 10) return BadRequest();

            if(text.Length > 2000)
                return BadRequest("Text je příliš dlouhý, maximálně 2000 znaků.");

            var predmet = await  _context.Predmets.Include("Hodnocenis").FirstOrDefaultAsync(x => x.ID == idPredmet);
            var studentName = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if(predmet.Hodnocenis.Any(x => x.studentName == studentName))
            {
                return BadRequest("K předmětu lze přidat pouze jedno hodnocení.");
            }

            var hodnoceni = new Hodnoceni
            {
                studentName = studentName,
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

        [HttpDelete("{idPredmet}/{hodnoceniID}")]
        [Authorize]
        public async Task<ActionResult<Hodnoceni>> DeleteRating(int idPredmet, int hodnoceniID)
        {
            var predmet = await  _context.Predmets.Include("Hodnocenis").FirstOrDefaultAsync(x => x.ID == idPredmet);

            var hodnoceni = predmet.Hodnocenis.FirstOrDefault(h => h.ID == hodnoceniID);

            if(hodnoceni.studentName != User.FindFirst(ClaimTypes.NameIdentifier)?.Value)
                return BadRequest("Nemáte oprávnění smazat toto hodnocení.");

            predmet.Hodnocenis.Remove(hodnoceni);

            if(await _context.SaveChangesAsync() > 0)
            {
                return hodnoceni;
            }
            return BadRequest();
        }

        [HttpPut("{idPredmet}/{hodnoceniID}")]
        [Authorize]
        public async Task<ActionResult<List<Hodnoceni>>> EditRating(int idPredmet, int hodnoceniID, [FromBody] string text)
        {
            if(text.Length > 2000)
                return BadRequest("Text je příliš dlouhý, maximálně 2000 znaků.");

            var predmet = await  _context.Predmets.Include("Hodnocenis").FirstOrDefaultAsync(x => x.ID == idPredmet);
            var hodnoceni = predmet.Hodnocenis.FirstOrDefault(h => h.ID == hodnoceniID);

            if(hodnoceni.studentName != User.FindFirst(ClaimTypes.NameIdentifier)?.Value)
                return BadRequest("Nemáte oprávnění upravovat toto hodnocení.");

            hodnoceni.text = text;

            if(await _context.SaveChangesAsync() > 0)
            {
                return predmet.Hodnocenis;
            }
            return BadRequest("Nebyly provedeny žádné změny.");
        }
    }
}