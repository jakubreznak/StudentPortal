using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace API.Entities
{
    public class Student : IdentityUser<int>
    {
        public string upolNumber { get; set; }
        public int oborIdno { get; set; }
        public int rocnikRegistrace { get; set; }
        public DateTime datumRegistrace { get; set; }
        public ICollection<AppUserRole> UserRoles { get; set; }
    }
}