using EasyBills.Core.Entity;
using EasyBills.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EasyBills.Infrastructure.Data.Repositories;

public class RepositoryBase<T> : IRepositoryBase<T> where T : Entity
{
    internal readonly ApplicationDbContext DbContext;

    internal readonly DbSet<T> DBSet;

    public RepositoryBase(ApplicationDbContext dbContext)
    {
        this.DbContext = dbContext;
        this.DBSet = dbContext.Set<T>();
    }

    public virtual async Task<IEnumerable<T>> GetAll(
        Expression<Func<T, bool>>? filter = null)
    {
        try
        {
            IQueryable<T> records = DBSet;

            if (filter is not null)
            {
                records = records.Where(filter);
            }

            return await records.ToListAsync();
        }
        catch (Exception ex)
        {
            var message = $"Failed retrieving records. Error: {ex}";
            throw new Exception(message);
        }
    }

    public virtual Task<T?> GetOne(Expression<Func<T, bool>> find)
    {
        try
        {
            return DBSet.FirstOrDefaultAsync(find);
        }
        catch (Exception ex)
        {
            var message = $"Failed retrieving record. Error: {ex}";
            throw new Exception(message);
        }
    }

    public virtual Task<T?> GetById(Guid id)
    {
        return GetOne(e => e.Id == id);
    }

    public void Add(T entity) => DbContext.Add(entity);

    public void Update(T entity) => DbContext.Update(entity);

    public void Remove(T entity) => DbContext.Remove(entity);

    public Task<int> SaveChanges() => DbContext.SaveChangesAsync();

    public void Dispose()
    {
        DbContext.Dispose();
        GC.SuppressFinalize(this);
    }
}