using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Karshare.API.Data;

public class GenericRepository<T>(AppDbContext context) where T : class
{
    private readonly DbSet<T> _dbSet = context.Set<T>();
    
    public async Task<T?> FindAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<T?> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.SingleOrDefaultAsync(predicate);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return _dbSet;
    }

    public async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> predicate)
    {
        return _dbSet.Where(predicate);
    }

    public async Task<T> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var T = await _dbSet.FindAsync(id);
        if (T == null)
            return false;

        _dbSet.Remove(T);
        return await context.SaveChangesAsync() > 0;
    }
}