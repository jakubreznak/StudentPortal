namespace API.Entities
{
    public class Soubor
    {
        public int ID { get; set; }
        public string Url { get; set; }
        public string PublicID { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
        public string studentName { get; set; }
        public string DateAdded { get; set; }
        public Predmet Predmet { get; set; }
        public int PredmetID { get; set; }
    }
}