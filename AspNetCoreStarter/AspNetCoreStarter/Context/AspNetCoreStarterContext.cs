using AspNetCoreStarter.Contracts.Enums;
using AspNetCoreStarter.Models;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreStarter.Context
{
    public class AspNetCoreStarterContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Todo> Todos { get; set; }

        public AspNetCoreStarterContext(DbContextOptions<AspNetCoreStarterContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasDefaultValue(Role.Regular);

            // add created and updated at to all entities
            modelBuilder.Entity<User>()
                .Property(u => u.CreatedAt)
                .HasDefaultValueSql("getdate()")
                .HasColumnType("datetime");

            modelBuilder.Entity<User>()
                .Property(u => u.UpdatedAt)
                .HasDefaultValueSql("getdate()")
                .HasColumnType("datetime");

            modelBuilder.Entity<Todo>()
                .Property(u => u.CreatedAt)
                .HasDefaultValueSql("getdate()")
                .HasColumnType("datetime");

            modelBuilder.Entity<Todo>()
                .Property(u => u.UpdatedAt)
                .HasDefaultValueSql("getdate()")
                .HasColumnType("datetime");
        }
    }
}
