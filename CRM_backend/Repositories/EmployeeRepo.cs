using CRM_backend.DB;
using CRM_backend.Models.Employee;
using Microsoft.EntityFrameworkCore;

namespace CRM_backend.Repositories
{
    public class EmployeeRepo : Repository<Employee>, IEmployeeRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Employee> _entities;
        private readonly ILogger<Repository<Employee>> _logger;
        public EmployeeRepo(ApplicationDbContext context, ILogger<Repository<Employee>> logger)
            : base(context, logger)
        {
            _context = context;
            _entities = _context.Set<Employee>();
            _logger = logger;
        }

        public async Task<Employee> SearchByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name cannot be null or empty.", nameof(name));
            }
            try
            {
                var val = await _entities.Where(e => e.FullName.ToLower() == name.ToLower())
                                .FirstOrDefaultAsync();
                if (val == null)
                {
                    throw new KeyNotFoundException($"Employee with name {name} not found.");
                }
                return val;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error searching for employee by name: {name}");
                throw; // rethrow to allow higher layers to catch

            }
        }
    }
}
