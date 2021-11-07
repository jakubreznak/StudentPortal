using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.HelpClass;
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
        public async Task<ActionResult<IEnumerable<string>>> GetStudents([FromQuery] StudentParams studentParams)
        {
            var students = await _userManager.Users.OrderByDescending(x => x.datumRegistrace).ToListAsync();
            if(!string.IsNullOrEmpty(studentParams.Nazev))
                students = students.Where(x => x.UserName.ToLower().Contains(studentParams.Nazev.ToLower())).ToList();
            List<string> studentsNames = new List<string>();
            foreach(var student in students){
                studentsNames.Add(student.UserName);
            }
            var pagedStudents = PagedList<string>.CreateFromList(studentsNames, studentParams.PageNumber, studentParams.PageSize);

            Response.AddPaginationHeader(pagedStudents.CurrentPage, pagedStudents.PageSize, pagedStudents.TotalCount, pagedStudents.TotalPages);
            return pagedStudents;
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpDelete("student/{name}")]
        public async Task<ActionResult<IEnumerable<string>>> DeleteStudent(string name)
        {
            var student = await _userManager.Users.SingleOrDefaultAsync(p => p.UserName == name);
            if(student == null) return BadRequest();
            var result = await _userManager.DeleteAsync(student);
            if(!result.Succeeded) return BadRequest();
            return Ok();
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("topics")]
        public async Task<ActionResult<IEnumerable<Topic>>> GetTopics([FromQuery] AdminTopicParams topicParams)
        {
            var topics = await _context.Topics.OrderByDescending(x => x.ID).ToListAsync();
            if(!string.IsNullOrEmpty(topicParams.Nazev))
            {
                topics = topics.Where(x => x.name.ToLower().Contains(topicParams.Nazev.ToLower())).ToList();
            }
            if(!string.IsNullOrEmpty(topicParams.Student))
            {
                topics = topics.Where(x => x.studentName.ToLower().Contains(topicParams.Student.ToLower())).ToList();
            }
            var pagedTopics = PagedList<Topic>.CreateFromList(topics, topicParams.PageNumber, topicParams.PageSize);

            Response.AddPaginationHeader(pagedTopics.CurrentPage, pagedTopics.PageSize, pagedTopics.TotalCount, pagedTopics.TotalPages);
            return pagedTopics;
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpDelete("topic/{id}")]
        public async Task<ActionResult<IEnumerable<Topic>>> DeleteTopic(int id)
        {
            var topic = await _context.Topics.SingleOrDefaultAsync(t => t.ID == id);
            if(topic == null) return BadRequest();
            _context.Topics.Remove(topic);
            if(await _context.SaveChangesAsync() <= 0) return BadRequest();
            return Ok();
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("comments")]
        public async Task<ActionResult<IEnumerable<Comment>>> GetComments([FromQuery] AdminCommentParams commentParams)
        {
            var topics = await _context.Topics.Include("comments").ToListAsync();
            List<Comment> comments = new List<Comment>();
            foreach(var topic in topics)
            {
                comments.AddRange(topic.comments);
            }

            foreach(var comment in comments)
            {
                comment.topic.comments = null;
            }

            if(!string.IsNullOrEmpty(commentParams.Nazev))
            {
                comments = comments.Where(x => x.text.ToLower().Contains(commentParams.Nazev.ToLower())).ToList();
            }
            if(!string.IsNullOrEmpty(commentParams.Student))
            {
                comments = comments.Where(x => x.studentName.ToLower().Contains(commentParams.Student.ToLower())).ToList();
            }
            if(!string.IsNullOrEmpty(commentParams.Tema))
            {
                comments = comments.Where(x => x.topic.name.ToLower().Contains(commentParams.Tema.ToLower())).ToList();
            }
            var pagedComments = PagedList<Comment>.CreateFromList(comments, commentParams.PageNumber, commentParams.PageSize);

            Response.AddPaginationHeader(pagedComments.CurrentPage, pagedComments.PageSize, pagedComments.TotalCount, pagedComments.TotalPages);
            return pagedComments;
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
                        return Ok();
                    }
                }
            }
            return BadRequest();
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("hodnoceni")]
        public async Task<ActionResult<IEnumerable<Hodnoceni>>> GetHodnoceni([FromQuery] AdminHodnoceniParams hodnoceniParams)
        {
            var predmets = await _context.Predmets.Include("Hodnocenis").ToListAsync();
            List<Hodnoceni> hodnocenis = new List<Hodnoceni>();
            foreach(var predmet in predmets)
            {
                hodnocenis.AddRange(predmet.Hodnocenis);
            }

            foreach(var hodnoceni in hodnocenis)
            {
                hodnoceni.predmet.Files = null;
                hodnoceni.predmet.Hodnocenis = null;
                hodnoceni.predmet.Students = null;
            }

            if(!string.IsNullOrEmpty(hodnoceniParams.Text))
            {
                hodnocenis = hodnocenis.Where(x => x.text.ToLower().Contains(hodnoceniParams.Text.ToLower())).ToList();
            }
            if(!string.IsNullOrEmpty(hodnoceniParams.Student))
            {
                hodnocenis = hodnocenis.Where(x => x.studentName.ToLower().Contains(hodnoceniParams.Student.ToLower())).ToList();
            }
            if(!string.IsNullOrEmpty(hodnoceniParams.Predmet))
            {
                hodnocenis = hodnocenis.Where(x => x.predmet.nazev.ToLower().Contains(hodnoceniParams.Predmet.ToLower())).ToList();        
            }
            var pagedHodnoceni = PagedList<Hodnoceni>.CreateFromList(hodnocenis, hodnoceniParams.PageNumber, hodnoceniParams.PageSize);

            Response.AddPaginationHeader(pagedHodnoceni.CurrentPage, pagedHodnoceni.PageSize, pagedHodnoceni.TotalCount, pagedHodnoceni.TotalPages);
            return pagedHodnoceni;
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
                        return Ok();
                    }
                }
            }
            return BadRequest();
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("soubory")]
        public async Task<ActionResult<IEnumerable<Soubor>>> GetMaterialy([FromQuery] AdminMaterialParams materialParams)
        {
            var predmets = await _context.Predmets.Include("Files").ToListAsync();
            List<Soubor> soubory = new List<Soubor>();
            foreach(var predmet in predmets)
            {
                soubory.AddRange(predmet.Files);
            }

            if(!string.IsNullOrEmpty(materialParams.Nazev))
            {
                soubory = soubory.Where(x => x.FileName.ToLower().Contains(materialParams.Nazev.ToLower())).ToList();
            }
            if(!string.IsNullOrEmpty(materialParams.Student))
            {
                soubory = soubory.Where(x => x.studentName.ToLower().Contains(materialParams.Student.ToLower())).ToList();
            }
            if(!string.IsNullOrEmpty(materialParams.Typ))
            {
                soubory = soubory.Where(x => x.Extension.ToLower().Contains(materialParams.Typ.ToLower())).ToList();
            }
            
            var pagedMaterials = PagedList<Soubor>.CreateFromList(soubory, materialParams.PageNumber, materialParams.PageSize);

            Response.AddPaginationHeader(pagedMaterials.CurrentPage, pagedMaterials.PageSize, pagedMaterials.TotalCount, pagedMaterials.TotalPages);
            return pagedMaterials;
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

                        return Ok();
                    }
                }
            }
            return BadRequest();
        }
    }

    
}