using System;
using System.Collections.Generic;
using System.Linq;
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
            return await _context.Topics.Where(t => t.predmetID == id).ToListAsync();
        }

        [HttpGet("topic/{id}")]
        [Authorize]
        public async Task<ActionResult<Topic>> GetCommentsByTopic (int id)
        {
            return await _context.Topics.Include("comments").FirstOrDefaultAsync(t => t.ID == id);
        }

        [HttpPost("{predmetID}/{studentName}")]
        [Authorize]
        public async Task<ActionResult<Topic>> NewTopic (string predmetID, string studentName, [FromBody] string topicName)
        {

            if(studentName == null || topicName == null || topicName.Length == 0)
                return BadRequest();

            var topic = new Topic
            {
                predmetID = predmetID ?? String.Empty,
                studentName = studentName,
                name = topicName,
                created = DateTime.Now.ToString("dd'.'MM'.'yyyy")
            };

            _context.Topics.Add(topic);
            await _context.SaveChangesAsync();

            return topic;
        }

        [HttpPost("comment/{topicID}/{name}")]
        [Authorize]
        public async Task<ActionResult<Topic>> PostComment (int? topicID, string name, [FromBody] string text)
        {
            if(topicID == null || text == null || name == null)
                return BadRequest();

            var topic = await _context.Topics.Include("comments").FirstOrDefaultAsync(t => t.ID == topicID);
            if(topic == null)
                return BadRequest();

            var comment = new Comment
            {
                topicID = topicID.Value,
                created = DateTime.Now.ToString("dd'.'MM'.'yyyy"),
                text = text,
                studentName = name
            };

            topic.comments.Add(comment);
            if(await _context.SaveChangesAsync() > 0)
            {
                return topic;
            }
            return BadRequest();
        }
    }
}