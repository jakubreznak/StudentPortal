using Microsoft.AspNetCore.Identity;

namespace API.Entities
{
    public class AppUserRole : IdentityUserRole<int>
    {
        public Student Student { get; set; }
        public AppRole Role { get; set; }
    }
}