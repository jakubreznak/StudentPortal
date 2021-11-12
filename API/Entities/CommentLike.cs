namespace API.Entities
{
    public class CommentLike
    {
        public int StudentId { get; set; }
        public Student Student { get; set; }
        public int CommentId { get; set; }
        public Comment Comment { get; set; }
    }
}