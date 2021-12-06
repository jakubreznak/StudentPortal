using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.CustomExtensions;
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
    public class DiscussionController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly UserManager<Student> _userManager;
        public DiscussionController(DataContext context, UserManager<Student> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Topic>>> GetTopics()
        {
            return await _context.Topics.ToListAsync();
        }

        [HttpGet("{id}")]
        [Authorize]
        public ActionResult<IEnumerable<Topic>> GetTopicsByPredmet(string id, [FromQuery] TopicParams topicParams)
        {
            List<Topic> topicsList = new List<Topic>(); 
            if(id != "x")
            {
                var predmet = _context.Predmets.FirstOrDefault(p => p.ID == Convert.ToInt32(id));
                var predmety = _context.Predmets.Where(p => p.katedra == predmet.katedra && p.zkratka == predmet.zkratka).ToList();            
                foreach(var pred in predmety)
                {
                    topicsList.AddRange(_context.Topics.Include(t => t.comments).Where(t => t.predmetID == pred.ID.ToString()));
                }
                topicsList = topicsList.Distinct().OrderByDescending(x => x.createdDateTime).ToList();                
            }
            else 
            {
                topicsList = _context.Topics.Include(t => t.comments).Where(x => x.predmetID == "x").OrderByDescending(x => x.createdDateTime).ToList();
            }
            int allItemsCount = topicsList.Count();
            
            if(!string.IsNullOrEmpty(topicParams.Nazev))
            {
                topicsList = topicsList.Where(x => x.name.ToLower().Contains(topicParams.Nazev.ToLower())).ToList();
            }
            if(!string.IsNullOrEmpty(topicParams.Student))
            {
                topicsList = topicsList.Where(x => x.studentName.RemoveAccentsToLower().Contains(topicParams.Student.RemoveAccentsToLower())).ToList();
            }

            switch (topicParams.OrderBy)
            {
                case "datum":
                    topicsList = topicsList.OrderByDescending(x => x.ID).ToList();
                    break;
                case "nazev":
                    topicsList = topicsList.OrderBy(x => x.name).ToList();
                    break;
                case "komentare":
                    topicsList = topicsList.OrderByDescending(x => x.comments.Count()).ToList();
                    break;
            }

            var topics = PagedList<Topic>.CreateFromList(topicsList, topicParams.PageNumber, topicParams.PageSize);

            Response.AddPaginationHeader(topics.CurrentPage, topics.PageSize, topics.TotalCount, topics.TotalPages, allItemsCount);
            return Ok(topics);
        }

        [HttpGet("topic/{id}")]
        [Authorize]
        public ActionResult<IEnumerable<Comment>> GetCommentsByTopic (int id, [FromQuery] CommentParams commentParams)
        {
            var comments = _context.Comments.Include(x => x.Replies).Where(x => x.topicID == id).ToList();
            int allItemsCount = comments.Count();
            if(!string.IsNullOrEmpty(commentParams.Nazev))
            {
                comments = comments.Where(x => x.text.ToLower().Contains(commentParams.Nazev.ToLower())).ToList();
            }
            if(!string.IsNullOrEmpty(commentParams.Student))
            {
                comments = comments.Where(x => x.studentName.RemoveAccentsToLower().Contains(commentParams.Student.RemoveAccentsToLower())).ToList();
            }

            foreach(var comment in comments)
            {
                comment.StudentsLikedBy = _context.CommentLikes.Where(x => x.CommentId == comment.ID).ToList();
                comment.StudentsLikedBy.ForEach(x => x.Student = null);
                comment.Replies = comment.Replies.OrderByDescending(x => x.ID).ToList();
            }

            switch (commentParams.OrderBy)
            {
                case "datum":
                    comments = comments.OrderByDescending(x => x.ID).ToList();
                    break;
                case "oblibenost":
                    comments = comments.OrderByDescending(x => x.StudentsLikedBy.Count()).ToList();
                    break;
                case "odpovedi":
                    comments = comments.OrderByDescending(x => x.Replies.Count()).ToList();
                    break;
            }

            var pagedComments = PagedList<Comment>.CreateFromList(comments, commentParams.PageNumber, commentParams.PageSize);

            Response.AddPaginationHeader(pagedComments.CurrentPage, pagedComments.PageSize, pagedComments.TotalCount, pagedComments.TotalPages, allItemsCount);
            return Ok(pagedComments);
        }

        [HttpGet("topicInfo/{id}")]
        [Authorize]
        public async Task<ActionResult<Topic>> GetTopicInfo (int id)
        {
            return await _context.Topics.FirstOrDefaultAsync(t => t.ID == id);
        }

        [HttpPost("{predmetID}")]
        [Authorize]
        public async Task<ActionResult<Topic>> NewTopic (string predmetID, [FromBody] string topicName)
        {

            if(topicName == null || topicName.Length == 0)
                return BadRequest();

            if(topicName.Length > 200)
                return BadRequest("Název tématu může mít maximálně 200 znaků.");

            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var student = _userManager.Users.FirstOrDefault(s => s.UserName == username);

            var topic = new Topic
            {
                predmetID = predmetID ?? String.Empty,
                studentName = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                name = topicName.Trim(),
                created = DateTime.Now.ToString("dd'.'MM'.'yyyy"),
                createdDateTime = DateTime.Now,
                accountName = student.accountName
            };

            _context.Topics.Add(topic);
            await _context.SaveChangesAsync();

            return topic;
        }

        [HttpPost("comment/{topicID}")]
        [Authorize]
        public async Task<ActionResult<Comment>> PostComment (int? topicID, [FromBody] string text)
        {
            if(topicID == null || text == null)
                return BadRequest();
            
            if(text.Length > 2000)
                return BadRequest("Text je příliš dlouhý, maximálně 2000 znaků.");

            var topic = await _context.Topics.Include("comments").FirstOrDefaultAsync(t => t.ID == topicID);
            if(topic == null)
                return BadRequest();

            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var student = _userManager.Users.FirstOrDefault(s => s.UserName == username);

            var comment = new Comment
            {
                topicID = topicID.Value,
                created = DateTime.Now.ToString("dd'.'MM'.'yyyy HH:mm"),
                text = text.Trim(),
                studentName = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                accountName = student.accountName
            };

            topic.comments.Add(comment);
            if(await _context.SaveChangesAsync() > 0)
            {
                return Ok(comment);
            }
            return BadRequest();
        }

        [HttpDelete("comment/{topicID}/{commentID}")]
        [Authorize]
        public async Task<ActionResult<Comment>> DeleteComment (int topicID, int commentID)
        {
            var comment = _context.Comments.Include(x => x.Replies).FirstOrDefault(c => c.ID == commentID);
            if(comment == null)
                return BadRequest();
            
            if(comment.studentName != User.FindFirst(ClaimTypes.NameIdentifier)?.Value)
                return BadRequest("Nemáte oprávnění smazat tento komentář.");

            if(comment.Replies != null && comment.Replies.Count() > 0)
                return BadRequest("Nelze smazat komentář s odpověďmi.");

            _context.Comments.Remove(comment);
            if(await _context.SaveChangesAsync() > 0)
            {
                return Ok(comment);
            }
            return BadRequest();
        }

        [HttpPost("reply/{commentId}")]
        [Authorize]
        public async Task<ActionResult<Reply>> PostReply (int? commentId, [FromBody] string text)
        {
            if(commentId == null || text == null)
                return BadRequest();
            
            if(text.Length > 500)
                return BadRequest("Text je příliš dlouhý, maximálně 500 znaků.");

            var comment = await _context.Comments.Include(x => x.Replies).FirstOrDefaultAsync(x => x.ID == commentId);
            if(comment == null)
                return BadRequest();

            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var student = _userManager.Users.FirstOrDefault(s => s.UserName == username);

            var reply = new Reply
            {
                commentId = commentId.Value,
                created = DateTime.Now.ToString("dd'.'MM'.'yyyy HH:mm"),
                text = text.Trim(),
                studentName = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                accountName = student.accountName
            };

            comment.Replies.Add(reply);
            if(await _context.SaveChangesAsync() > 0)
            {
                return Ok(reply);
            }
            return BadRequest();
        }

        [HttpDelete("reply/{replyId}")]
        [Authorize]
        public async Task<ActionResult> DeleteReply (int replyId)
        {
            var reply = await _context.Replies.FirstOrDefaultAsync(x => x.ID == replyId);            
            
            if(reply.studentName != User.FindFirst(ClaimTypes.NameIdentifier)?.Value)
                return BadRequest("Nemáte oprávnění smazat tuto odpověď.");

            _context.Replies.Remove(reply);
            if(await _context.SaveChangesAsync() > 0)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpDelete("{topicID}")]
        [Authorize]
        public async Task<ActionResult<Topic>> DeleteTopic (int topicID)
        {
            var topic = await _context.Topics.Include("comments").FirstOrDefaultAsync(t => t.ID == topicID);
            if(topic == null)
                return BadRequest();

            if(topic.studentName != User.FindFirst(ClaimTypes.NameIdentifier)?.Value)
                return BadRequest("Nemáte oprávnění smazat toto téma.");

            if(topic.comments.Any())
                return BadRequest("Nelze smazat téma u kterého jsou odpovědi.");

            _context.Topics.Remove(topic);
            await _context.SaveChangesAsync();

            return topic;
        }

        [HttpPut("comment/{topicID}/{commentID}")]
        [Authorize]
        public async Task<ActionResult<Topic>> EditComment(int topicID, int commentID, [FromBody] string text)
        {
            if(text.Length > 2000)
                return BadRequest("Text je příliš dlouhý, maximálně 2000 znaků.");

            var topic = await _context.Topics.Include("comments").FirstOrDefaultAsync(t => t.ID == topicID);
            if(topic == null)
                return BadRequest();

            var comment = topic.comments.FirstOrDefault(c => c.ID == commentID);
            if(comment == null)
                return BadRequest();

            if(comment.text == text.Trim())
                return BadRequest("Nebyly provedeny žádné změny.");
            
            if(comment.studentName != User.FindFirst(ClaimTypes.NameIdentifier)?.Value)
                return BadRequest("Nemáte oprávnění upravit tento komentář.");

            if(comment.Replies != null && comment.Replies.Count() > 0)
                return BadRequest("Nelze upravit komentář s odpověďmi.");

            comment.text = text.Trim();
            comment.edited = DateTime.Now.ToString("dd'.'MM'.'yyyy HH:mm");

            _context.Comments.Update(comment);

            if(await _context.SaveChangesAsync() > 0)
            {
                return topic;
            }
            return BadRequest("Nebyly provedeny žádné změny.");
        }

        [HttpPut("reply/{replyId}")]
        [Authorize]
        public async Task<ActionResult> EditReply(int replyId, [FromBody] string text)
        {
            if(text.Length > 500)
                return BadRequest("Text je příliš dlouhý, maximálně 500 znaků.");

            var reply = await _context.Replies.FirstOrDefaultAsync(x => x.ID == replyId);
            if(reply == null)
                return BadRequest();
            
            if(reply.studentName != User.FindFirst(ClaimTypes.NameIdentifier)?.Value)
                return BadRequest("Nemáte oprávnění upravit tuto odpvěď.");

            if(reply.text == text.Trim())
                return BadRequest("Nebyly provedeny žádné změny.");

            reply.text = text.Trim();
            reply.edited = DateTime.Now.ToString("dd'.'MM'.'yyyy HH:mm");

            if(await _context.SaveChangesAsync() > 0)
            {
                return Ok();
            }
            return BadRequest("Nebyly provedeny žádné změny.");
        }

        [HttpGet("reply/{commentId}/{repliesCount}")]
        [Authorize]
        public ActionResult<IEnumerable<Reply>> LoadMoreReplies (int commentId, int repliesCount)
        {
            var replies = _context.Replies.Where(x => x.commentId == commentId).OrderByDescending(x => x.ID).ToList();

            replies = replies.Skip(3 * repliesCount).Take(3).ToList();
            return Ok(replies);
        }
    }
}