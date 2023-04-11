using EasyBills.Domain.Entities;
using EasyBills.Infrastructure.Data.Repositories;

namespace EasyBills.Domain.Interfaces;

/// <summary>
/// Category repository interface.
/// </summary>
public interface ICategoryRepository : IRepositoryBase<Category>
{
}
