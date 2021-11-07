namespace API.HelpClass
{
    public class AdminHodnoceniParams : PaginationParams
    {
        public string Text { get; set; }
        public string Student { get; set; }
        public string Predmet { get; set; }
    }
}