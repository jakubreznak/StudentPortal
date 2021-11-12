namespace API.HelpClass
{
    public class HodnoceniParams : PaginationParams
    {
        public string OrderBy { get; set; } = "datum";
    }
}