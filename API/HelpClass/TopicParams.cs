namespace API.HelpClass
{
    public class TopicParams : PaginationParams
    {
        public string Nazev { get; set; }
        public string Student { get; set; }
        public string OrderBy { get; set; } = "datum";
    }
}