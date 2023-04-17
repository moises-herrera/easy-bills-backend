using EasyBills.Core.Entity;
using EasyBills.Domain.Entities.Enums;

namespace EasyBills.Domain.Entities;

/// <summary>
/// Represents an account entity.
/// </summary>
public class Account : Entity
{
    /// <summary>
    /// Initialize a new instance of <see cref="Account"/> class.
    /// </summary>
    /// <param name="name">Name.</param>
    /// <param name="typeAccount">Type of account.</param>
    /// <param name="balance">Balance amount.</param>
    public Account(string name, FinanceAccountType typeAccount, decimal balance, Guid userId)
    {
        Name = name;
        TypeAccount = typeAccount;
        Balance = balance;
        UserId = userId;
        Transactions = new List<Transaction>();
    }

    /// <summary>
    /// Name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Type of account.
    /// </summary>
    public FinanceAccountType TypeAccount { get; set; }

    /// <summary>
    /// Current balance.
    /// </summary>
    public decimal Balance { get; set; }

    /// <summary>
    /// User id.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Account owner.
    /// </summary>
    public User User { get; set; }

    /// <summary>
    /// Transactions.
    /// </summary>
    public List<Transaction> Transactions { get; set; }
}
