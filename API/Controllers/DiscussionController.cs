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
    public class DiscussionController : BaseApiController
    {
        private readonly DataContext _context;
        public DiscussionController(DataContext context)
        {
            _context = context;
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
                    var topic = _context.Topics.Include(t => t.comments).Where(t => t.predmetID == pred.ID.ToString());
                    topicsList.AddRange(topic);
                }
                topicsList = topicsList.OrderByDescending(x => x.createdDateTime).ToList();
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
                topicsList = topicsList.Where(x => x.studentName.ToLower().Contains(topicParams.Student.ToLower())).ToList();
            }

            var topics = PagedList<Topic>.CreateFromList(topicsList, topicParams.PageNumber, topicParams.PageSize);

            Response.AddPaginationHeader(topics.CurrentPage, topics.PageSize, topics.TotalCount, topics.TotalPages, allItemsCount);
            return Ok(topics);
        }

        [HttpGet("topic/{id}")]
        [Authorize]
        public ActionResult<IEnumerable<Comment>> GetCommentsByTopic (int id, [FromQuery] CommentParams commentParams)
        {
            var comments = _context.Topics.Include("comments").FirstOrDefault(t => t.ID == id).comments;
            int allItemsCount = comments.Count();
            if(!string.IsNullOrEmpty(commentParams.Nazev))
            {
                comments = comments.Where(x => x.text.ToLower().Contains(commentParams.Nazev.ToLower())).ToList();
            }
            if(!string.IsNullOrEmpty(commentParams.Student))
            {
                comments = comments.Where(x => x.studentName.ToLower().Contains(commentParams.Student.ToLower())).ToList();
            }

            foreach(var comment in comments)
            {
                comment.topic = null;
                comment.StudentsLikedBy = _context.CommentLikes.Where(x => x.CommentId == comment.ID).ToList();
                comment.StudentsLikedBy.ForEach(x => x.Student = null);
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

            var topic = new Topic
            {
                predmetID = predmetID ?? String.Empty,
                studentName = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                name = topicName,
                created = DateTime.Now.ToString("dd'.'MM'.'yyyy"),
                createdDateTime = DateTime.Now
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

            var comment = new Comment
            {
                topicID = topicID.Value,
                created = DateTime.Now.ToString("dd'.'MM'.'yyyy HH:mm"),
                text = text,
                studentName = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
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
            var topic = await _context.Topics.Include("comments").FirstOrDefaultAsync(t => t.ID == topicID);
            if(topic == null)
                return BadRequest();

            var comment = topic.comments.FirstOrDefault(c => c.ID == commentID);
            if(comment == null)
                return BadRequest();
            
            if(comment.studentName != User.FindFirst(ClaimTypes.NameIdentifier)?.Value)
                return BadRequest("Nemáte oprávnění smazat tento komentář.");

            topic.comments.Remove(comment);
            if(await _context.SaveChangesAsync() > 0)
            {
                return Ok(comment);
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
            
            if(comment.studentName != User.FindFirst(ClaimTypes.NameIdentifier)?.Value)
                return BadRequest("Nemáte oprávnění upravit tento komentář.");

            comment.text = text;

            if(await _context.SaveChangesAsync() > 0)
            {
                return topic;
            }
            return BadRequest("Nebyly provedeny žádné změny.");
        }
    }
}