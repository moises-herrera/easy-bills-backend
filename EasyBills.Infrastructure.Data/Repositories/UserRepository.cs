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
}
