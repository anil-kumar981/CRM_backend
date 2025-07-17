using CRM_backend.DTO;
using CRM_backend.Models;
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
            
        }
    }
}
