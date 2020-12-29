using System;

namespace API.Entities
{
    public class Student
    {
        public int ID { get; set; }
        public string name { get; set; }
        public byte[] passwordHash { get; set; }
        public byte[] passwordSalt {get; set;}
        public string upolNumber { get; set; }
        public int oborIdno { get; set; }
        public int rocnikRegistrace { get; set; }
        public DateTime datumRegistrace { get; set; }
    }
}