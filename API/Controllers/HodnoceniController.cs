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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class HodnoceniController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly UserManager<Student> _userManager;
        public HodnoceniController(DataContext context, UserManager<Student> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet("{idPredmet}")]
        [Authorize]
        public ActionResult<List<Hodnoceni>> GetHodnoceniPredmetu(int idPredmet, [FromQuery] HodnoceniParams hodnoceniParams)
        {
            List<Hodnoceni> hodnoceniList = new List<Hodnoceni>(); 
            var predmet = _context.Predmets.FirstOrDefault(p => p.ID == idPredmet);
            var predmety = _context.Predmets.Where(p => p.katedra == predmet.katedra && p.zkratka == predmet.zkratka).ToList();            
            foreach(var pred in predmety)
            {
                hodnoceniList.AddRange(_context.Hodnoceni.Where(x => x.predmetID == pred.ID).ToList());
            }

            hodnoceniList = hodnoceniList.Distinct().ToList();
            
            foreach(var hod in hodnoceniList)
            {
                hod.StudentsLikedBy = _context.HodnoceniLikes.Where(x => x.HodnoceniId == hod.ID).ToList();
                hod.StudentsLikedBy.ForEach(x => x.Student = null);
            }
            int allItemsCount = hodnoceniList.Count(); 

            switch (hodnoceniParams.OrderBy)
            {
                case "datum":
                    hodnoceniList = hodnoceniList.OrderByDescending(x => x.ID).ToList();
                    break;
                case "ohodnoceni":
                    hodnoceniList = hodnoceniList.OrderByDescending(x => x.rating).ToList();
                    break;
                case "oblibenost":
                    hodnoceniList = hodnoceniList.OrderByDescending(x => x.StudentsLikedBy.Count()).ToList();
                    break;
            }

            var pagedHodnoceni = PagedList<Hodnoceni>.CreateFromList(hodnoceniList, hodnoceniParams.PageNumber, hodnoceniParams.PageSize);

            Response.AddPaginationHeader(pagedHodnoceni.CurrentPage, pagedHodnoceni.PageSize, pagedHodnoceni.TotalCount, pagedHodnoceni.TotalPages, allItemsCount);
            return Ok(pagedHodnoceni);
        }

        [HttpGet("cislo/{idPredmet}")]
        [Authorize]
        public ActionResult<int> GetRatingNumber(int idPredmet)
        {
            List<Hodnoceni> hodnoceniList = new List<Hodnoceni>(); 
            var predmet = _context.Predmets.FirstOrDefault(p => p.ID == idPredmet);
            var predmety = _context.Predmets.Where(p => p.katedra == predmet.katedra && p.zkratka == predmet.zkratka).ToList();            
            foreach(var pred in predmety)
            {
                hodnoceniList.AddRange(_context.Hodnoceni.Where(x => x.predmetID == pred.ID).ToList());
            }

            hodnoceniList = hodnoceniList.Distinct().ToList();
            int ratingNumber = 0;
            foreach(var hod in hodnoceniList)
            {
                ratingNumber += hod.rating;
            }

            return Ok(Math.Ceiling((double) ratingNumber / hodnoceniList.Count()));
        }

        [HttpPost("{idPredmet}/{cislo}")]
        [Authorize]
        public async Task<ActionResult<List<Hodnoceni>>> PostRating(int idPredmet, int cislo, [FromBody] string text)
        {
            if(cislo < 1 || cislo > 10) return BadRequest();

            if(text.Length > 2000)
                return BadRequest("Text je příliš dlouhý, maximálně 2000 znaků.");

            var predmet = await  _context.Predmets.Include(x => x.Hodnocenis).FirstOrDefaultAsync(x => x.ID == idPredmet);
            var studentName = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var student = _userManager.Users.FirstOrDefault(s => s.UserName == studentName);

            if(predmet.Hodnocenis.Any(x => x.studentName == studentName))
            {
                return BadRequest("K předmětu lze přidat pouze jedno hodnocení.");
            }

            var hodnoceni = new Hodnoceni
            {
                studentName = studentName,
                accountName = String.IsNullOrEmpty(student.accountName) ? studentName : student.accountName,
                text = text.Trim(),
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
            var hodnoceni = _context.Hodnoceni.FirstOrDefault(h => h.ID == hodnoceniID);

            if(hodnoceni.studentName != User.FindFirst(ClaimTypes.NameIdentifier)?.Value)
                return BadRequest("Nemáte oprávnění smazat toto hodnocení.");

            _context.Hodnoceni.Remove(hodnoceni);

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

            if(hodnoceni.text == text.Trim())
                return BadRequest("Nebyly provedeny žádné změny.");

            hodnoceni.text = text.Trim();
            hodnoceni.edited = DateTime.Now.ToString("dd'.'MM'.'yyyy HH:mm");


            if(await _context.SaveChangesAsync() > 0)
            {
                return predmet.Hodnocenis;
            }
            return BadRequest("Nebyly provedeny žádné změny.");
        }
    }
}