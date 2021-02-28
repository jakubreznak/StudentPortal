using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class RegisterDTO
    {
        [Required]
        public string name { get; set; }
        
        [Required]
        public string password { get; set; }

        [Required]
        public string upolNumber {get; set;}
    }
}