using FieldManagementSystem.Entities;
using Microsoft.EntityFrameworkCore;

namespace FieldManagementSystem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Field> Fields => Set<Field>();
        public DbSet<Controller> Controllers => Set<Controller>();
        public DbSet<UserController> UserControllers => Set<UserController>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Max size for Email
            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .HasMaxLength(320)
                .IsRequired();


            // Unique Email
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Many-to-many key
            modelBuilder.Entity<UserController>()
                .HasKey(uc => new { uc.UserId, uc.ControllerId });
        }

    }
}
