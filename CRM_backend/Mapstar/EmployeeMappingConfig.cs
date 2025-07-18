using CRM_backend.DTO.EmployeeDtos;
using CRM_backend.DTO.ProjectDtos;
using CRM_backend.Models.Employee;
using CRM_backend.Models.Project;
using Mapster;

namespace CRM_backend.Mapstar
{
    public static class EmployeeMappingConfig
    {
        public static void RegisterMappings()
        {
            TypeAdapterConfig<EmployeeDto, Employee>.NewConfig()
                .Ignore(dest => dest.Id)
                .Ignore(dest => dest.UserTechnologies);
            TypeAdapterConfig<ProjectDto, Project>.NewConfig().Ignore(dest => dest.Document);
        }
    }
}
