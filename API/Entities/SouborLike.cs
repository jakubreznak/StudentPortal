namespace API.Entities
{
    public class SouborLike
    {
        public int StudentId { get; set; }
        public Student Student { get; set; }
        public int SouborId { get; set; }
        public Soubor Soubor { get; set; }
    }
}