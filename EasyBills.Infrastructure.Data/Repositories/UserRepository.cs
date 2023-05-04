using EasyBills.Domain.Entities;
using EasyBills.Domain.Interfaces;
using EasyBills.Infrastructure.Data.Context;

namespace EasyBills.Infrastructure.Data.Repositories;

/// <summary>
/// User repository to manage user records.
/// </summary>
public class UserRepository : RepositoryBase<User>, IUserRepository
{
    /// <summary>
    /// Initialize a new instance of <see cref="UserRepository"/> class.
    /// </summary>
    /// <param name="context">App db context.</param>
    public UserRepository(ApplicationDbContext context)
        : base(context)
    {
        
    }

    /// <summary>
    /// Check if the current user is admin.
    /// </summary>
    /// <param name="userId">The current user id.</param>
    /// <returns>If the user is admin.</returns>
    public async Task<bool> IsUserAdmin(Guid userId)
    {
        var user = await GetById(userId);

        return user is not null && user.IsAdmin;
    }
}
