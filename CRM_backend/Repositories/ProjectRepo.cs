using CRM_backend.DB;

using CRM_backend.Models.Project;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace CRM_backend.Repositories
{
    public class ProjectRepo : Repository<Project>, IProjectRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Project> _entities;
        private readonly ILogger<Repository<Project>> _logger;
       

    

        public ProjectRepo(ApplicationDbContext context, ILogger<Repository<Project>> logger) : base(context, logger)
        {
            _context = context;
            _entities = _context.Set<Project>();
            _logger = logger;
        }

        public async Task<Project> SearchByName(string name)
        {
            try
            {
                if (name == null || string.IsNullOrEmpty(name))
                {
                    throw new ArgumentException("Name cannot be null or empty.", nameof(name));
                }
                var project = await _entities
                    .Where(p => p.Title.ToLower() == name.ToLower())
                    .FirstOrDefaultAsync();
                if (project == null)
                {
                    _logger.LogWarning($"Project with name {name} not found.");
                    throw new KeyNotFoundException($"Project with name {name} not found.");
                }
                return project;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error searching for project by name: {name}");
                throw; // rethrow to allow higher layers to catch
            }
        }
        public async Task<Project> AddWithTechnologiesAsync(Project project, List<int> technologyIds)
        {
            if (technologyIds != null && technologyIds.Any())
            {
                if (project.ProjectTechnologies == null)
                    project.ProjectTechnologies = new List<ProjectTechnology>();
                foreach (var techId in technologyIds)
                {
                    project.ProjectTechnologies.Add(new ProjectTechnology
                    {
                        TechnologyId = techId,
                        Project = project
                    });
                }
            }
            await _entities.AddAsync(project);
            await _context.SaveChangesAsync();
            return project;
        }

        public async Task<Project> UpdateWithTechnologiesAsync(int id, Project updatedProject, List<int> technologyIds)
        {
            var existingProject = await _entities
                .Include(p => p.ProjectTechnologies)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (existingProject == null)
                return null;
           

            // Update main project fields (like title, description etc.)
            _context.Entry(existingProject).CurrentValues.SetValues(updatedProject);

            if (existingProject.ProjectTechnologies == null)
                existingProject.ProjectTechnologies = new List<ProjectTechnology>();
            else
                existingProject.ProjectTechnologies.Clear();

            // Add new technologies
            if (technologyIds != null && technologyIds.Any())
            {
                foreach (var techId in technologyIds)
                {
                    existingProject.ProjectTechnologies.Add(new ProjectTechnology
                    {
                        TechnologyId = techId,
                        ProjectId = id
                    });
                }
            }

            await _context.SaveChangesAsync();
            return existingProject;
        }


    }
}
