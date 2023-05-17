using EasyBills.Core.Entity;
using EasyBills.Core.Interfaces;
using EasyBills.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace EasyBills.Infrastructure.Data.Repositories;

/// <summary>
/// Repository base.
/// </summary>
/// <typeparam name="T">Type of entity.</typeparam>
public class RepositoryBase<T> : IRepositoryBase<T> where T : Entity
{
    /// <summary>
    /// Application db context.
    /// </summary>
    internal readonly ApplicationDbContext DbContext;

    /// <summary>
    /// Db set of entities.
    /// </summary>
    internal readonly DbSet<T> DBSet;

    /// <summary>
    /// Initialize a new instance of <see cref="RepositoryBase{T}"/> class.
    /// </summary>
    /// <param name="dbContext">App db context.</param>
    public RepositoryBase(ApplicationDbContext dbContext)
    {
        this.DbContext = dbContext;
        this.DBSet = dbContext.Set<T>();
    }

    /// <summary>
    /// Get all the records.
    /// </summary>
    /// <param name="filter">Filter expression.</param>
    /// <param name="include">The properties to include.</param>
    /// <param name="pageNumber">The page number.</param>
    /// <param name="pageSize">The page size.</param>
    /// <returns>All records.</returns>
    /// <exception cref="Exception"></exception>
    public virtual async Task<IEnumerable<T>> GetAll(
        Expression<Func<T, bool>>? filter = null,
        string include = "", 
        int pageNumber = 1, 
        int pageSize = 10,
        string orderBy = "",
        bool orderAsc = false)
    {
        try
        {
            IQueryable<T> records = DBSet;

            if (filter is not null)
            {
                records = records.Where(filter);
            }

            if (!string.IsNullOrEmpty(orderBy))
            {
                var param = Expression.Parameter(typeof(T));
                var memberAccess = Expression.Property(param, orderBy);
                var convertedMemberAccess = Expression.Convert(memberAccess, typeof(object));
                var orderPredicate = Expression.Lambda<Func<T, object>>(convertedMemberAccess, param);
                records = orderAsc
                    ? records.OrderBy(orderPredicate)
                    : records.OrderByDescending(orderPredicate);
            }

            var includedProperties = include.Split(
                new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var property in includedProperties)
            {
                records = records.Include(property);
            }

            return await records.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        }
        catch (Exception ex)
        {
            var message = $"Failed retrieving records. Error: {ex}";
            throw new Exception(message);
        }
    }

    /// <summary>
    /// Get one record.
    /// </summary>
    /// <param name="find">Find expression.</param>
    /// <returns>A record.</returns>
    /// <exception cref="Exception"></exception>
    public virtual Task<T?> GetOne(Expression<Func<T, bool>> find, string include = "")
    {
        try
        {
            IQueryable<T> record = DBSet;

            var includedProperties = include.Split(
                new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var property in includedProperties)
            {
                record = record.Include(property);
            }

            return record.FirstOrDefaultAsync(find);
        }
        catch (Exception ex)
        {
            var message = $"Failed retrieving record. Error: {ex}";
            throw new Exception(message);
        }
    }

    /// <summary>
    /// Get a record by id.
    /// </summary>
    /// <param name="id">Id.</param>
    /// <returns>A record.</returns>
    public virtual Task<T?> GetById(Guid id, string include = "")
    {
        return GetOne(e => e.Id == id, include);
    }

    /// <summary>
    /// Add a record.
    /// </summary>
    /// <param name="entity">Entity.</param>
    public void Add(T entity) => DbContext.Add(entity);

    /// <summary>
    /// Update a record.
    /// </summary>
    /// <param name="entity">Entity.</param>
    public void Update(T entity) => DbContext.Update(entity);

    /// <summary>
    /// Remove a record.
    /// </summary>
    /// <param name="entity">Entity.</param>
    public void Remove(T entity) => DbContext.Remove(entity);

    /// <summary>
    /// Save changes.
    /// </summary>
    /// <returns>The number of entries affected.</returns>
    public Task<int> SaveChanges() => DbContext.SaveChangesAsync();

    /// <summary>
    /// Dispose.
    /// </summary>
    public void Dispose()
    {
        DbContext.Dispose();
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Count all the records
    /// </summary>
    /// <returns>The number of all the records.</returns>
    public Task<int> Count()
    {
        return DBSet.CountAsync();
    }
}