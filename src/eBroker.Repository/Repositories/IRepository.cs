using System.Linq.Expressions;

namespace eBroker.Data.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetAsync(int id);
        IQueryable<T> GetAll();
        IQueryable<T> Find(Expression<Func<T, bool>> predicate);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);

    }
}
