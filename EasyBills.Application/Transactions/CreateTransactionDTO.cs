namespace EasyBills.Application.Transactions;

/// <summary>
/// Trasaction DTO for creation or updates.
/// </summary>
/// <param name="Amount">Amount.</param>
/// <param name="Description">Description.</param>
/// <param name="AccountId">Account id.</param>
/// <param name="CategoryId">Category id.</param>
/// <param name="IsIncome">If is an income.</param>
public record CreateTransactionDTO(
    decimal Amount,
    string Description,
    Guid AccountId,
    Guid CategoryId,
    bool IsIncome
    );
