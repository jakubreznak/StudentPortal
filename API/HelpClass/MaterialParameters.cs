namespace API.HelpClass
{
    public class MaterialParameters : PaginationParams
    {
        public string Nazev { get; set; }
        public string Typ { get; set; }
        public string OrderBy { get; set; } = "datum";
    }
}