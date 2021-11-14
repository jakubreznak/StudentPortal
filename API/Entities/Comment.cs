using System;
using System.Collections.Generic;

namespace API.Entities
{
    public class Comment
    {
        public Comment()
        {
            this.StudentsLikedBy = new List<CommentLike>();
        }

        public int ID { get; set; }
        public string text { get; set; }
        public string created { get; set; }
        public string studentName { get; set; }
        public string edited { get; set; }
        public int topicID { get; set; }
        public Topic topic { get; set; }
        public List<CommentLike> StudentsLikedBy { get; set; }
        public List<Reply> Replies { get; set; }
    }
}