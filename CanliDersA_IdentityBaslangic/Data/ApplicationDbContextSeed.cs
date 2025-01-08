using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CanliDersA_IdentityBaslangic.Data
{
    public static class ApplicationDbContextSeed
    {
        public static async  Task SeedUserAsync(UserManager<IdentityUser> userManager)
        {
            var ornekKullanici = new IdentityUser()
            {
                Email = "ornek@example.com",
                UserName = "ornek@example.com",
                EmailConfirmed = true
            };

            await userManager.CreateAsync(ornekKullanici, "Ankara1.");
        }

        public static async Task SeedAdminRoleAndUserAsync(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            if (!await roleManager.RoleExistsAsync("admin"))
            {
                var adminRole = new IdentityRole("admin");
                await roleManager.CreateAsync(adminRole);
            }
            
            if (!await userManager.Users.AnyAsync(u => u.Email == "admin@example.com"))
            {
                var adminKullanici = new IdentityUser()
                {
                    Email = "admin@example.com",
                    UserName = "admin@example.com",
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(adminKullanici, "P@ssword1");

                await userManager.AddToRoleAsync(adminKullanici, "admin");
            
            }
        }
    }
}
