using System;
using System.Collections.Generic;

namespace API.Entities
{
    public class Topic
    {
        public Topic() => this.comments = new List<Comment>();
        public int ID { get; set; }
        public string name { get; set; }
        public string predmetID { get; set; }
        public string studentName { get; set; }
        public string accountName { get; set; }
        public string created { get; set; }
        public DateTime createdDateTime { get; set; }
        public List<Comment> comments { get; set; }
    }
}