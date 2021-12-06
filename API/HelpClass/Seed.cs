using System.Collections.Generic;
using System.Threading.Tasks;
using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.HelpClass
{
    public class Seed
    {
        public static async Task SeedUsersAndRoles(UserManager<Student> userManager,
            RoleManager<AppRole> roleManager)
            {
                if (await userManager.Users.AnyAsync()) return;

                var roles = new List<AppRole>
                {
                    new AppRole{Name = "Member"},
                    new AppRole{Name = "Admin"}
                };

                foreach (var role in roles)
                {
                    await roleManager.CreateAsync(role);
                }

                var admin = new Student
                {
                    UserName = "jakub",
                    accountName = "jakub",
                    oborIdno = 45
                };

                await userManager.CreateAsync(admin, "Kjkszpj92");
                await userManager.AddToRoleAsync(admin, "Admin");
            }
    }
}