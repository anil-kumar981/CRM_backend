using Microsoft.EntityFrameworkCore;
using multivalue_input.Models;

namespace multivalue_input.db
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Technology> Technologies { get; set; }
        public DbSet<UserTechnology> UserTechnologies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserTechnology>()
                .HasKey(ut => new { ut.UserId, ut.TechnologyId });
            modelBuilder.Entity<UserTechnology>().HasOne(ut=> ut.User)
                .WithMany(u => u.UserTechnologies)
                .HasForeignKey(ut => ut.UserId);
            modelBuilder.Entity<UserTechnology>().HasOne(ut => ut.Technology).WithMany(ut=> ut.UserTechnologies)
                .HasForeignKey(ut => ut.TechnologyId);
        }
    }
}
