using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using API.Entities;

namespace API.DTOs
{
    public class RegisterDTO
    {
        [Required]
        public string name { get; set; }
        
        [Required]
        public string password { get; set; }

        public string upolNumber {get; set;}

        [Required]
        public List<Predmet> predmety {get; set;}
        
        [Required]
        public int oborIdno { get; set; }
        
        [Required]
        public int rocnikRegistrace { get; set; }
    }
}