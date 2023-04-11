using EasyBills.Domain.Entities;
using EasyBills.Infrastructure.Data.Repositories;

namespace EasyBills.Domain.Interfaces;

/// <summary>
/// User repository interface.
/// </summary>
public interface IUserRepository : IRepositoryBase<User>
{
}
