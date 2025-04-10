using System.Linq.Expressions;

namespace Karshare.API.Repositories
{
    public interface IRepository<T, Tid> where T : new()
    {
        Task<T> Add(T entity);
        Task<T?> GetById(Tid id);
        Task<T?> Get(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> GetAll();
        Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> predicate);
        Task<T?> Update(T entity);
        Task<bool> Delete(Tid id);
    }
}
