using Microsoft.EntityFrameworkCore;

namespace Logs.Migrations
{
    public class AppDbContext : DbContext
    {
        public DbSet<Entry>? Entries { get; set; }
        public DbSet<User>? Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=sqlite.db");
        }
    }
}
