using EasyBills.Domain.Entities;
using EasyBills.Domain.Interfaces;
using EasyBills.Infrastructure.Data.Context;

namespace EasyBills.Infrastructure.Data.Repositories;

/// <summary>
/// Transaction repository to handle transaction actions.
/// </summary>
public class TransactionRepository : RepositoryBase<Transaction>, ITransactionRepository
{
    /// <summary>
    /// Initialize a new instance of <see cref="TransactionRepository"/> class.
    /// </summary>
    /// <param name="dbContext">App db context.</param>
    public TransactionRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}
