using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Sakenny.Core.Models;

namespace Sakenny.Repository.Data.DataSeed
{
    public static class AdminSeeding
    {
        public static async Task SeedRolesAdmin(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, SakennyDbContext sakennyDb)
        {
            var email = "the.eoa96@gmail.com";
            var us = sakennyDb.Users.FirstOrDefault(u => u.Email == email);

            if (us == null) return;
            string role = "Admin";
            var ex = await sakennyDb.Roles.AnyAsync(u => u.Name == role);
            if (!ex)
                await roleManager.CreateAsync(new IdentityRole(role));

            var admin = sakennyDb.Roles.FirstOrDefault(ur => ur.Name == role);
            var rl = await sakennyDb.UserRoles.AnyAsync(ur => ur.UserId == us.Id);
            if (!rl)
            {
                await userManager.AddToRoleAsync(us, role);
            }


        }



    }
}
