using System.Collections.Generic;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class StudentsController : BaseApiController
    {
        private readonly DataContext _context;
        public StudentsController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            return await _context.Students.ToListAsync();
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<UserDTO>> GetStudent(string name)
        {
            var student =  await _context.Students.SingleOrDefaultAsync(p => p.name == name);
            return new UserDTO()
            {
                ID = student.ID,
                name = student.name,
                upolNumber = student.upolNumber,
                oborIdno = student.oborIdno,
                rocnikRegistrace = student.rocnikRegistrace,
                datumRegistrace = student.datumRegistrace
            };
        }
    }
}