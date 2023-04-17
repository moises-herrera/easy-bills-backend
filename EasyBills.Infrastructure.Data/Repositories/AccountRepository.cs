using EasyBills.Domain.Entities;
using EasyBills.Domain.Interfaces;
using EasyBills.Infrastructure.Data.Context;

namespace EasyBills.Infrastructure.Data.Repositories;

/// <summary>
/// Account repository to handle account actions.
/// </summary>
public class AccountRepository : RepositoryBase<Account>, IAccountRepository
{
    /// <summary>
    /// Initialize a new instance of <see cref="AccountRepository"/> class.
    /// </summary>
    /// <param name="dbContext">App db context.</param>
    public AccountRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}
