using CRM_backend.DB;
using CRM_backend.Models.Employee;

namespace CRM_backend.Repositories
{
    public class TechnologyRepo : Repository<Technologies>, ITechnologyRepo
    {
        public TechnologyRepo(ApplicationDbContext context, ILogger<Repository<Technologies>> logger) : base(context, logger)
        {
        }
    }
}
