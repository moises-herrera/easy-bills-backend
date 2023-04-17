using EasyBills.Core.Interfaces;
using EasyBills.Domain.Entities;

namespace EasyBills.Domain.Interfaces;

/// <summary>
/// Transactions repository interface.
/// </summary>
public interface ITransactionRepository : IRepositoryBase<Transaction>
{
}
