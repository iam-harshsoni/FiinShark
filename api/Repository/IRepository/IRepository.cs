using System.Linq.Expressions;

namespace api.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetAsync(Expression<Func<T,bool>> filter);
        Task AddAsync(T entity);
        void Remove(T entity);
        void RemoveRange(List<T> entities);
    }
}
