using EasyBills.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EasyBills.Infrastructure.Data.Repositories;

public interface IRepositoryBase<T> where T : IEntity
{
    Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>>? filter = null);
    Task<T?> GetById(Guid id);
    Task<T?> GetOne(Expression<Func<T, bool>> find);
    void Add(T entity);
    void Remove(T entity);
    void Update(T entity);
    Task<int> SaveChanges();
    void Dispose();
}