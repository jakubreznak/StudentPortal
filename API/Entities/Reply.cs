namespace API.Entities
{
    public class Reply
    {
        public int ID { get; set; }
        public string text { get; set; }
        public string created { get; set; }
        public string studentName { get; set; }
        public string accountName { get; set; }
        public string edited { get; set; }
        public int commentId { get; set; }
        public Comment comment { get; set; }
    }
}