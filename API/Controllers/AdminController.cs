using System.Collections.Generic;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using API.Services;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AdminController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly UserManager<Student> _userManager;
        private readonly IFileService _fileService;

        public AdminController(DataContext context, UserManager<Student> userManager, IFileService fileService)
        {
            _context = context;
            _userManager = userManager;
            _fileService = fileService;
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("students")]
        public async Task<ActionResult<IEnumerable<string>>> GetStudents()
        {
            var students = await _userManager.Users.ToListAsync();
            List<string> studentsNames = new List<string>();
            foreach(var student in students){
                studentsNames.Add(student.UserName);
            }
            return studentsNames;
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpDelete("student/{name}")]
        public async Task<ActionResult<IEnumerable<string>>> DeleteStudent(string name)
        {
            var student = await _userManager.Users.SingleOrDefaultAsync(p => p.UserName == name);
            if(student == null) return BadRequest();
            var result = await _userManager.DeleteAsync(student);
            if(!result.Succeeded) return BadRequest();
            return await GetStudents();
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("topics")]
        public async Task<ActionResult<IEnumerable<Topic>>> GetTopics()
        {
            return await _context.Topics.ToListAsync();
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpDelete("topic/{id}")]
        public async Task<ActionResult<IEnumerable<Topic>>> DeleteStudent(int id)
        {
            var topic = await _context.Topics.SingleOrDefaultAsync(t => t.ID == id);
            if(topic == null) return BadRequest();
            _context.Topics.Remove(topic);
            if(await _context.SaveChangesAsync() <= 0) return BadRequest();
            return await GetTopics();
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("comments")]
        public async Task<ActionResult<IEnumerable<Comment>>> GetComments()
        {
            var topics = await _context.Topics.Include("comments").ToListAsync();
            List<Comment> comments = new List<Comment>();
            foreach(var topic in topics)
            {
                foreach(var comment in topic.comments)
                {
                    comments.Add(comment);
                }
            }
            return comments;
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpDelete("comment/{id}")]
        public async Task<ActionResult<IEnumerable<Comment>>> DeleteComment(int id)
        {
            var topics = await _context.Topics.Include("comments").ToListAsync();
            List<Comment> comments = new List<Comment>();
            foreach(var topic in topics)
            {
                foreach(var comment in topic.comments)
                {
                    if(comment.ID == id)
                    {
                        topic.comments.Remove(comment);
                        if(await _context.SaveChangesAsync() <= 0) return BadRequest();
                        return await GetComments();
                    }
                }
            }
            return BadRequest();
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("hodnoceni")]
        public async Task<ActionResult<IEnumerable<Hodnoceni>>> GetHodnoceni()
        {
            var predmets = await _context.Predmets.Include("Hodnocenis").ToListAsync();
            List<Hodnoceni> hodnocenis = new List<Hodnoceni>();
            foreach(var predmet in predmets)
            {
                foreach(var hodnoceni in predmet.Hodnocenis)
                {
                    hodnocenis.Add(hodnoceni);
                }
            }
            return hodnocenis;
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpDelete("hodnoceni/{id}")]
        public async Task<ActionResult<IEnumerable<Hodnoceni>>> DeleteHodnoceni(int id)
        {
            var predmets = await _context.Predmets.Include("Hodnocenis").ToListAsync();
            List<Hodnoceni> hodnocenis = new List<Hodnoceni>();
            foreach(var predmet in predmets)
            {
                foreach(var hodnoceni in predmet.Hodnocenis)
                {
                    if(hodnoceni.ID == id)
                    {
                        predmet.Hodnocenis.Remove(hodnoceni);
                        if(await _context.SaveChangesAsync() <= 0) return BadRequest();
                        return await GetHodnoceni();
                    }
                }
            }
            return BadRequest();
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("soubory")]
        public async Task<ActionResult<IEnumerable<Soubor>>> GetMaterialy()
        {
            var predmets = await _context.Predmets.Include("Files").ToListAsync();
            List<Soubor> soubory = new List<Soubor>();
            foreach(var predmet in predmets)
            {
                foreach(var soubor in predmet.Files)
                {
                    soubory.Add(soubor);
                }
            }
            return soubory;
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpDelete("soubor/{id}")]
        public async Task<ActionResult<IEnumerable<Soubor>>> DeleteMaterial(int id)
        {
            var predmets = await _context.Predmets.Include("Files").ToListAsync();
            List<Soubor> soubory = new List<Soubor>();
            foreach(var predmet in predmets)
            {
                foreach(var soubor in predmet.Files)
                {
                    if(soubor.ID == id)
                    {
                        predmet.Files.Remove(soubor);
                        if(await _context.SaveChangesAsync() <= 0) return BadRequest();

                        var result = await _fileService.RemoveFileAsync(soubor.PublicID);
                         if (result.Error != null) return BadRequest();

                        return await GetMaterialy();
                    }
                }
            }
            return BadRequest();
        }
    }

    
}