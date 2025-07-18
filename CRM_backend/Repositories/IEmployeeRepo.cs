using CRM_backend.Models.Employee;

namespace CRM_backend.Repositories
{
    public interface IEmployeeRepo : IRepositories<Employee>
    {
        Task<Employee> SearchByName(string name);
    }
}
