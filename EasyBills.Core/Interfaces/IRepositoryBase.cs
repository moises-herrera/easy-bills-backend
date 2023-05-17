using EasyBills.Core.Entity;
using System.Linq.Expressions;

namespace EasyBills.Core.Interfaces;

/// <summary>
/// Repository base interface.
/// </summary>
/// <typeparam name="T">Type of the entity.</typeparam>
public interface IRepositoryBase<T> where T : IEntity
{
    /// <summary>
    /// Get all the records.
    /// </summary>
    /// <param name="filter">Filter expression.</param>
    /// <param name="include">Properties to include.</param>
    /// <returns>All the records.</returns>
    Task<IEnumerable<T>> GetAll(
        Expression<Func<T, bool>>? filter = null, 
        string include = "", 
        int pageNumber = 1, 
        int pageSize = 10, 
        string orderBy = "",
        bool orderAsc = false);

    /// <summary>
    /// Count all the records.
    /// </summary>
    /// <returns>The number of all the records.</returns>
    Task<int> Count();

    /// <summary>
    /// Get a record by id.
    /// </summary>
    /// <param name="id">Id.</param>
    /// <param name="include">Properties to include.</param>
    /// <returns>A record.</returns>
    Task<T?> GetById(Guid id, string include = "");

    /// <summary>
    /// Get one record.
    /// </summary>
    /// <param name="find">Find expression.</param>
    /// <param name="include">Properties to include.</param>
    /// <returns>A record.</returns>
    Task<T?> GetOne(Expression<Func<T, bool>> find, string include = "");

    /// <summary>
    /// Add a record.
    /// </summary>
    /// <param name="entity">Entity.</param>
    void Add(T entity);

    /// <summary>
    /// Update a record.
    /// </summary>
    /// <param name="entity">Entity.</param>
    void Update(T entity);

    /// <summary>
    /// Remove a record.
    /// </summary>
    /// <param name="entity">Entity</param>
    void Remove(T entity);

    /// <summary>
    /// Save changes.
    /// </summary>
    /// <returns>The number of entires affected.</returns>
    Task<int> SaveChanges();

    /// <summary>
    /// Dispose.
    /// </summary>
    void Dispose();
}