using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
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
        public async Task<ActionResult<IEnumerable<Topic>>> GetTopicsByPredmet(string id)
        {
            return await _context.Topics.Include("comments").Where(t => t.predmetID == id).ToListAsync();
        }

        [HttpGet("topic/{id}")]
        [Authorize]
        public async Task<ActionResult<Topic>> GetCommentsByTopic (int id)
        {
            return await _context.Topics.Include("comments").FirstOrDefaultAsync(t => t.ID == id);
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
                created = DateTime.Now.ToString("dd'.'MM'.'yyyy")
            };

            _context.Topics.Add(topic);
            await _context.SaveChangesAsync();

            return topic;
        }

        [HttpPost("comment/{topicID}")]
        [Authorize]
        public async Task<ActionResult<Topic>> PostComment (int? topicID, [FromBody] string text)
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
                return topic;
            }
            return BadRequest();
        }

        [HttpDelete("comment/{topicID}/{commentID}")]
        [Authorize]
        public async Task<ActionResult<Topic>> DeleteComment (int topicID, int commentID)
        {
            var topic = await _context.Topics.Include("comments").FirstOrDefaultAsync(t => t.ID == topicID);
            if(topic == null)
                return BadRequest();

            var comment = topic.comments.FirstOrDefault(c => c.ID == commentID);
            if(comment == null)
                return BadRequest();

            topic.comments.Remove(comment);
            if(await _context.SaveChangesAsync() > 0)
            {
                return topic;
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

            if(topic.comments.Any())
                return BadRequest("Nelze smazat téma  u kterého jsou odpovědi.");

            _context.Topics.Remove(topic);
            await _context.SaveChangesAsync();

            return topic;
        }
    }
}