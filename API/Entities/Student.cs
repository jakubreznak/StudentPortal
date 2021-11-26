using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace API.Entities
{
    public class Student : IdentityUser<int>
    {
        public Student()
        {
            this.predmetyStudenta = new List<Predmet>();
            this.likedMaterialy = new List<SouborLike>();
            this.likedComments = new List<CommentLike>();
            this.likedHodnoceni = new List<HodnoceniLike>();
        }

        public string upolNumber { get; set; }
        public string accountName { get; set; }
        public int? oborIdno { get; set; }
        public int? rocnikRegistrace { get; set; }
        public List<Predmet> predmetyStudenta { get; set; }
        public List<SouborLike> likedMaterialy { get; set; }
        public List<CommentLike> likedComments { get; set; }
        public List<HodnoceniLike> likedHodnoceni { get; set; }
        public DateTime datumRegistrace { get; set; }
        public ICollection<AppUserRole> UserRoles { get; set; }
    }
}