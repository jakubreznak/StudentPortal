namespace API.Entities
{
    public class Hodnoceni
    {
        public int ID { get; set; }
        public string text { get; set; }
        public int rating { get; set; }
        public string studentName { get; set; }
        public string created { get; set; }
        public int predmetID { get; set; }
        public Predmet predmet { get; set; }
    }
}