using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class LikeController : BaseApiController
    {
        private readonly DataContext _context;
         private readonly UserManager<Student> _userManager;
        public LikeController(UserManager<Student> userManager, DataContext context)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost("material")]
        [Authorize]
        public async Task<ActionResult<int>> LikeMaterial([FromBody]int idSoubor)
        {
            var soubor = _context.Soubor.FirstOrDefault(x => x.ID == idSoubor);

            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var student = _userManager.Users.FirstOrDefault(s => s.UserName == username);

            if(_context.SouborLikes.Any(x => x.SouborId == soubor.ID && x.StudentId == student.Id))
                return BadRequest("Nelze dát like vícekrát stejnému materiálu.");

            var souborLike = new SouborLike
            {
                SouborId = soubor.ID,
                StudentId = student.Id
            };

            soubor.StudentsLikedBy.Add(souborLike);

            if(await _context.SaveChangesAsync() > 0)
            {
                return Ok(idSoubor);
            }
            return BadRequest("Něco se nepovedlo.");
        }

        [HttpDelete("material/{idSoubor}")]
        [Authorize]
        public async Task<ActionResult> RemoveLikeMaterial(int idSoubor)
        {
            var soubor = _context.Soubor.FirstOrDefault(x => x.ID == idSoubor);

            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var student = _userManager.Users.FirstOrDefault(s => s.UserName == username);

            var souborLike = _context.SouborLikes.FirstOrDefault(x => x.SouborId == soubor.ID && x.StudentId == student.Id);
            if(souborLike == null) return BadRequest("Tento materiál nemá váš like.");                

            soubor.StudentsLikedBy.Remove(souborLike);

            if(await _context.SaveChangesAsync() > 0)
            {
                return Ok();
            }
            return BadRequest("Něco se nepovedlo.");
        }

        [HttpPost("comment")]
        [Authorize]
        public async Task<ActionResult<int>> LikeComment([FromBody]int idComment)
        {
            var comment = _context.Comments.FirstOrDefault(x => x.ID == idComment);

            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var student = _userManager.Users.FirstOrDefault(s => s.UserName == username);

            if(_context.CommentLikes.Any(x => x.CommentId == comment.ID && x.StudentId == student.Id))
                return BadRequest("Nelze dát like vícekrát stejnému komentáři.");

            var commentLike = new CommentLike
            {
                CommentId = comment.ID,
                StudentId = student.Id
            };

            comment.StudentsLikedBy.Add(commentLike);

            if(await _context.SaveChangesAsync() > 0)
            {
                return Ok(idComment);
            }
            return BadRequest("Něco se nepovedlo.");
        }

        [HttpDelete("comment/{idComment}")]
        [Authorize]
        public async Task<ActionResult> RemoveLikeComment(int idComment)
        {
            var comment = _context.Comments.FirstOrDefault(x => x.ID == idComment);

            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var student = _userManager.Users.FirstOrDefault(s => s.UserName == username);

            var commentLike = _context.CommentLikes.FirstOrDefault(x => x.CommentId == comment.ID && x.StudentId == student.Id);
            if(commentLike == null) return BadRequest("Tento materiál nemá váš like.");                

            comment.StudentsLikedBy.Remove(commentLike);

            if(await _context.SaveChangesAsync() > 0)
            {
                return Ok();
            }
            return BadRequest("Něco se nepovedlo.");
        }


        [HttpPost("hodnoceni")]
        [Authorize]
        public async Task<ActionResult<int>> LikeHodnoceni([FromBody]int idHodnoceni)
        {
            var hodnoceni = _context.Hodnoceni.FirstOrDefault(x => x.ID == idHodnoceni);

            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var student = _userManager.Users.FirstOrDefault(s => s.UserName == username);

            if(_context.HodnoceniLikes.Any(x => x.HodnoceniId == hodnoceni.ID && x.StudentId == student.Id))
                return BadRequest("Nelze dát like vícekrát stejnému hodnocení.");

            var hodnoceniLike = new HodnoceniLike
            {
                HodnoceniId = hodnoceni.ID,
                StudentId = student.Id
            };

            hodnoceni.StudentsLikedBy.Add(hodnoceniLike);

            if(await _context.SaveChangesAsync() > 0)
            {
                return Ok(idHodnoceni);
            }
            return BadRequest("Něco se nepovedlo.");
        }

        [HttpDelete("hodnoceni/{idHodnoceni}")]
        [Authorize]
        public async Task<ActionResult> RemoveLikeHodnoceni(int idHodnoceni)
        {
            var hodnoceni = _context.Hodnoceni.FirstOrDefault(x => x.ID == idHodnoceni);

            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var student = _userManager.Users.FirstOrDefault(s => s.UserName == username);

            var hodnoceniLike = _context.HodnoceniLikes.FirstOrDefault(x => x.HodnoceniId == hodnoceni.ID && x.StudentId == student.Id);
            if(hodnoceniLike == null) return BadRequest("Toto hodnocení nemá váš like.");                

            hodnoceni.StudentsLikedBy.Remove(hodnoceniLike);

            if(await _context.SaveChangesAsync() > 0)
            {
                return Ok();
            }
            return BadRequest("Něco se nepovedlo.");
        }
    }
}