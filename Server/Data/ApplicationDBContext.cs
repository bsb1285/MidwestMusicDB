using Microsoft.EntityFrameworkCore;
using MidwestMusicDB.Shared.Models;


namespace MidwestMusicDB.Server.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options):base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        modelBuilder.Entity<User>()
            .HasKey(u => u.username);
        }
        public DbSet<User> Users { get; set; }

    }
}