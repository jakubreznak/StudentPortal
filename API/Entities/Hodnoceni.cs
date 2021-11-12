using System.Collections.Generic;

namespace API.Entities
{
    public class Hodnoceni
    {
        public Hodnoceni()
        {
            this.StudentsLikedBy = new List<HodnoceniLike>();
        }

        public int ID { get; set; }
        public string text { get; set; }
        public int rating { get; set; }
        public string studentName { get; set; }
        public string created { get; set; }
        public int predmetID { get; set; }
        public Predmet predmet { get; set; }
        public List<HodnoceniLike> StudentsLikedBy { get; set; }
    }
}