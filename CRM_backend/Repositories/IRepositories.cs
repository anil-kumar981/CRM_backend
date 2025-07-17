using System.Linq.Expressions;

namespace CRM_backend.Repositories
{
    public interface IRepositories<T>         where T : class
    {
        Task<T> GetByIdAsync(Expression<Func<T, bool>> filter);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
       
    }
}
