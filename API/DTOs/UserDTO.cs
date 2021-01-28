using System;

namespace API.DTOs
{
    public class UserDTO
    {
        public int ID { get; set; }
        public string name { get; set; }
        public string upolNumber { get; set; }
        public int oborIdno { get; set; }
        public int rocnikRegistrace { get; set; }
        public DateTime datumRegistrace { get; set; }
    }
}