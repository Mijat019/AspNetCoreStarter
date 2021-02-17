using AspNetCoreStarter.Models;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreStarter.Context
{
    public class AspNetCoreStarterContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public AspNetCoreStarterContext(DbContextOptions<AspNetCoreStarterContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(u => u.CreatedAt)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<User>()
                .Property(u => u.UpdatedAt)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}
