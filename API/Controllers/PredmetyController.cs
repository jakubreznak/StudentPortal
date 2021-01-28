using System.Collections.Generic;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace API.Controllers
{
    public class PredmetyController : BaseApiController
    {
        private readonly DataContext _context;
        public PredmetyController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Predmet>>> GetPredmety()
        {
            return await _context.Predmets.ToListAsync();
        }

        [HttpGet]
        [Route("getbyid/{id}")]
        public async Task<ActionResult<Predmet>> GetPredmet(int id)
        {
            return await _context.Predmets.FindAsync(id);
        }

        [HttpGet]
        [Route("getbyobor/{idObor}")]
        public async Task<ActionResult<IEnumerable<Predmet>>> GetPredmetyByObor(int idObor)
        {
            return await _context.Predmets.Where(p => p.oborIdNum == idObor).ToListAsync();
        }
    }
}