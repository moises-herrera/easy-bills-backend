using EasyBills.Core.Interfaces;
using EasyBills.Domain.Entities;

namespace EasyBills.Domain.Interfaces;

/// <summary>
/// User repository interface.
/// </summary>
public interface IUserRepository : IRepositoryBase<User>
{
    Task<bool> IsUserAdmin(Guid userId);
}
