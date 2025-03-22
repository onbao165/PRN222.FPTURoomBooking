using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PRN222.Assignment.FPTURoomBooking.Repositories.Models;

namespace PRN222.Assignment.FPTURoomBooking.Repositories.Repositories;

/// <summary>
/// Generic repository implementation
/// </summary>
public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : class
{
    protected readonly DbSet<TEntity> DbSet;

    public GenericRepository(ApplicationDbContext context)
    {
        DbSet = context.Set<TEntity>();
    }

    public IQueryable<TEntity> GetQueryable()
    {
        return DbSet.AsQueryable();
    }

    public async Task<TEntity?> GetByIdAsync(TKey id)
    {
        return await DbSet.FindAsync(id);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> expression)
    {
        return await DbSet.Where(expression).ToListAsync();
    }

    public void Add(TEntity entity)
    {
        DbSet.Add(entity);
    }

    public void AddRange(IEnumerable<TEntity> entities)
    {
        DbSet.AddRange(entities);
    }

    public void Update(TEntity entity)
    {
        DbSet.Update(entity);
    }

    public void Remove(TEntity entity)
    {
        DbSet.Remove(entity);
    }

    public void RemoveRange(IEnumerable<TEntity> entities)
    {
        DbSet.RemoveRange(entities);
    }
}