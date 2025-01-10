using Microsoft.EntityFrameworkCore;
using Pratik2_Jwt.Model;

namespace Pratik2_Jwt.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}
