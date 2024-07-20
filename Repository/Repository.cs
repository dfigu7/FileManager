// FileManager.DataAccess/Repositories/Repository.cs

using System.Linq.Expressions;
using DataAccess;
using DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Repository;

namespace Repositories;

public class Repository<T>(FileManagerDbContext context) : IRepository<T>
    where T : class
{
    protected readonly FileManagerDbContext _context = context;
    protected readonly DbSet<T> _dbSet = context.Set<T>();

    public async Task<T> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id) ?? throw new InvalidOperationException();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public void Remove(T entity)
    {
        _dbSet.Remove(entity);
    }
}