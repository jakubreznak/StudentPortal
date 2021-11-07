namespace API.HelpClass
{
    public class AdminCommentParams : PaginationParams
    {
        public string Nazev { get; set; }
        public string Student { get; set; }
        public string Tema { get; set; }
    }
}