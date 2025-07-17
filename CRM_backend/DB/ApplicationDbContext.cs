using CRM_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace CRM_backend.DB
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

       public DbSet<Employee> employees { get; set; }
       public DbSet<Technologies> Technologies { get; set; }
      public  DbSet<UserTechnologies> UserTechnologies { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserTechnologies>().HasKey(ut => new { ut.UserId, ut.TechnologyId });
            modelBuilder.Entity<UserTechnologies>().HasOne(u => u.Employee).WithMany(u => u.UserTechnologies).HasForeignKey(u => u.UserId);
            modelBuilder.Entity<UserTechnologies>()
     .HasOne(u => u.Technologies)
     .WithMany(t => t.UserTechnologies) // ✅ Correct: t is Technology model
     .HasForeignKey(u => u.TechnologyId);
        }
    }
}
