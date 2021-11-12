namespace API.HelpClass
{
    public class CommentParams : PaginationParams
    {
        public string Nazev { get; set; }
        public string Student { get; set; }
        public string OrderBy { get; set; } = "datum";
    }
}