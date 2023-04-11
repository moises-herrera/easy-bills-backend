using EasyBills.Core.Entity;

namespace EasyBills.Domain.Entities;

/// <summary>
/// Represents a transaction entity.
/// </summary>
public class Transaction : Entity
{
    /// <summary>
    /// Initialize a new instance of <see cref="Transaction"/> class.
    /// </summary>
    /// <param name="amount">Amount.</param>
    /// <param name="description">Description.</param>
    /// <param name="userId">User id.</param>
    /// <param name="accountId">Account id.</param>
    /// <param name="categoryId">Category id.</param>
    /// <param name="isIncome">If is an income.</param>
    public Transaction(
        decimal amount,
        string description,
        Guid userId,
        Guid accountId,
        Guid categoryId,
        bool isIncome)
    {
        Amount = amount;
        Description = description;
        UserId = userId;
        AccountId = accountId;
        CategoryId = categoryId;
        CreatedDate = DateTime.UtcNow;
        IsIncome = isIncome;
    }

    /// <summary>
    /// Amount.
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Description.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// User id.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Account id.
    /// </summary>
    public Guid AccountId { get; set; }

    /// <summary>
    /// Category id.
    /// </summary>
    public Guid CategoryId { get; set; }

    /// <summary>
    /// Created date of the transaction.
    /// </summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// If is an income.
    /// </summary>
    public bool IsIncome { get; set; }
}
