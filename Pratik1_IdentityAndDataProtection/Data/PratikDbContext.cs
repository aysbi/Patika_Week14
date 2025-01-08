using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Pratik1_IdentityAndDataProtection.Data
{
    public class PratikDbContext : IdentityDbContext<IdentityUser>
    {
        public PratikDbContext(DbContextOptions<PratikDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var hasher = new PasswordHasher<IdentityUser>();

            var user = new IdentityUser
            {
                Id = "1",
                UserName = "example",
                Email = "example@test.com",
                EmailConfirmed = true
            };

            user.PasswordHash = hasher.HashPassword(user, "P@ssword1");

            modelBuilder.Entity<IdentityUser>().HasData(user);
        }
    }
}
