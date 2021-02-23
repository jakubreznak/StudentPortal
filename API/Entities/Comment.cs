using System;

namespace API.Entities
{
    public class Comment
    {
        public int ID { get; set; }
        public string text { get; set; }
        public string created { get; set; }
        public string studentName { get; set; }
        public int topicID { get; set; }
        public Topic topic { get; set; }
    }
}