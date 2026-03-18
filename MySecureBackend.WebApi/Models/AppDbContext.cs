using Microsoft.EntityFrameworkCore;

namespace MySecureBackend.WebApi.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Environment2D> Environments { get; set; }
        public DbSet<Object2D> Objects { get; set; }
    }
}
