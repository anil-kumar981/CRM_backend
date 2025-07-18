

using CRM_backend.Models.Project;

namespace CRM_backend.Repositories
{
  public  interface  IProjectRepo : IRepositories<Project>
    {
        Task<Project> SearchByName(string name);
        Task<Project> AddWithTechnologiesAsync(Project project, List<int> technologyIds);
        Task<Project> UpdateWithTechnologiesAsync(int id, Project project, List<int> technologyIds);

    }
}
