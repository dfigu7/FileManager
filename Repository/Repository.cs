// FileManager.DataAccess/Repositories/Repository.cs

using System.Linq.Expressions;
using DataAccess;
using DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly FileManagerDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(FileManagerDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
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