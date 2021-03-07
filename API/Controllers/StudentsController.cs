using System.Collections.Generic;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class StudentsController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly UserManager<Student> _userManager;
        public StudentsController(DataContext context, UserManager<Student> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpGet("{name}")]
        [Authorize]
        public async Task<ActionResult<int>> GetOborIdByStudentName(string name)
        {
            var student = await _userManager.Users.SingleOrDefaultAsync(p => p.UserName == name);
            if(student == null) return BadRequest();
            return student.oborIdno;
        }
    }
}