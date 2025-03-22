using System.Linq.Expressions;

namespace PRN222.Assignment.FPTURoomBooking.Repositories.Repositories;

/// <summary>
/// Generic repository interface for basic CRUD operations
/// </summary>
public interface IGenericRepository<TEntity, in TKey> where TEntity : class
{
    IQueryable<TEntity> GetQueryable();
    Task<TEntity?> GetByIdAsync(TKey id);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> expression);
    void Add(TEntity entity);
    void AddRange(IEnumerable<TEntity> entities);
    void Update(TEntity entity);
    void Remove(TEntity entity);
}