using CRM_backend.Models.Employee;
using CRM_backend.Models.Project;
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
        public DbSet<Project> Projects { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserTechnologies>().HasKey(ut => new { ut.UserId, ut.TechnologyId });
            modelBuilder.Entity<UserTechnologies>().HasOne(u => u.Employee).WithMany(u => u.UserTechnologies).HasForeignKey(u => u.UserId);
            modelBuilder.Entity<UserTechnologies>()
     .HasOne(u => u.Technologies)
     .WithMany(t => t.UserTechnologies) 
     .HasForeignKey(u => u.TechnologyId);
            modelBuilder.Entity<Project>().Property(p => p.Status).HasConversion<string>();
            modelBuilder.Entity<ProjectTechnology>().HasKey(pt => new { pt.ProjectId, pt.TechnologyId });
            modelBuilder.Entity<ProjectTechnology>()
                .HasOne(pt => pt.Project)
                .WithMany(p => p.ProjectTechnologies)
                .HasForeignKey(pt => pt.ProjectId);
            modelBuilder.Entity<ProjectTechnology>().HasOne(pt=> pt.Technology)
                .WithMany(t => t.ProjectTechnologies)
                .HasForeignKey(pt => pt.TechnologyId);
        }
       
    }
}
