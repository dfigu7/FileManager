// FileManager.DataAccess/Repositories/IRepository.cs

using System.Linq.Expressions;

namespace Repository;

public interface IRepository<T> where T : class
{
    Task<T> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    Task AddAsync(T entity);
    void Remove(T entity);
    void Update(T entity);
    Task<int> CountAsync();
    Task SaveChangesAsync();
    Task DeleteAsync();
    Task SaveAsync();
    Task ReadAllBytesAsync();
    

}