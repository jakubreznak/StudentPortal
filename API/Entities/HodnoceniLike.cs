namespace API.Entities
{
    public class HodnoceniLike
    {
        public int StudentId { get; set; }
        public Student Student { get; set; }
        public int HodnoceniId { get; set; }
        public Hodnoceni Hodnoceni { get; set; }
    }
}