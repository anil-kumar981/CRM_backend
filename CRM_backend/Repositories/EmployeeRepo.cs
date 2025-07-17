using CRM_backend.DB;
using CRM_backend.Models;

namespace CRM_backend.Repositories
{
    public class EmployeeRepo: Repository<Employee>, IEmployeeRepo
    {
        public EmployeeRepo(ApplicationDbContext context, ILogger<Repository<Employee>> logger)
            : base(context, logger)
        {
        }
    }
}
