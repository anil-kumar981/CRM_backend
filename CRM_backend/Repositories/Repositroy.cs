using CRM_backend.DB;
using Microsoft.EntityFrameworkCore;

using System.Linq.Expressions;

namespace CRM_backend.Repositories
{
    /// <summary>
    /// Generic repository for basic CRUD operations.
    /// </summary>
    public class Repository<T> : IRepositories<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _entities;
        private readonly ILogger<Repository<T>> _logger;

        public Repository(ApplicationDbContext context, ILogger<Repository<T>> logger)
        {
            _context = context;
            _entities = _context.Set<T>();
            _logger = logger;
        }

        /// <summary>
        /// Add a new entity asynchronously.
        /// </summary>
        public async Task AddAsync(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity), "Entity cannot be null");

                await _entities.AddAsync(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error adding entity of type {typeof(T).Name}");
                throw; // rethrow to allow higher layers to catch
            }
        }

        /// <summary>
        /// Delete an entity by ID asynchronously.
        /// </summary>
        public async Task DeleteAsync(int id)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("Invalid ID", nameof(id));

                var entity = await _entities.FindAsync(id);
                if (entity == null)
                    throw new KeyNotFoundException($"Entity with ID {id} not found");

                _entities.Remove(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting entity with ID {id} of type {typeof(T).Name}");
                throw;
            }
        }

        /// <summary>
        /// Get all entities.
        /// </summary>
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            try
            {
                var entities = await _entities.ToListAsync();
                if (entities == null || !entities.Any())
                    throw new KeyNotFoundException($"No entities found for type {typeof(T).Name}");

                return entities;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving all entities of type {typeof(T).Name}");
                throw;
            }
        }

        /// <summary>
        /// Get a single entity by filter expression.
        /// </summary>
        public async Task<T> GetByIdAsync(Expression<Func<T, bool>> filter)
        {
            try
            {
                var entity = await _entities.FirstOrDefaultAsync(filter);
                if (entity == null)
                    throw new KeyNotFoundException($"Entity not found for type {typeof(T).Name}");

                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving entity by filter for type {typeof(T).Name}");
                throw;
            }
        }

        /// <summary>
        /// Update an existing entity.
        /// </summary>
        public async Task UpdateAsync(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity), "Entity cannot be null");

                _entities.Update(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating entity of type {typeof(T).Name}");
                throw;
            }
        }
    }
}
