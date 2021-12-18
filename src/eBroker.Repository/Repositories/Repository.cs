using eBroker.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace eBroker.Data.Repositories
{
    public class Repository<T> : IRepository<T> where T : class, new()
    {
        private readonly IEBrokerDbContext _ebrokerDbContext;
        private readonly DbSet<T> _dbSet;
        public Repository(IEBrokerDbContext ebrokerDbContext)
        {
            _ebrokerDbContext = ebrokerDbContext;
            _dbSet = _ebrokerDbContext.Set<T>();
        }

        public async Task<T> GetAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public IQueryable<T> GetAll()
        {
            return _dbSet;
        }

        public async Task<T> AddAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{typeof(T).Name} entity must not be null");
            }

            try
            {
                await _ebrokerDbContext.AddAsync(entity);
                await _ebrokerDbContext.SaveChangesAsync();

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entity)} could not be saved: {ex.Message}");
            }
        }

        public IQueryable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where<T>(predicate);
        }

        public async Task<T> UpdateAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{typeof(T).Name} entity must not be null");
            }

            try
            {
                 _ebrokerDbContext.Update(entity);
                await _ebrokerDbContext.SaveChangesAsync();

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entity)} could not be saved: {ex.Message}");
            }
        }
    }
}
