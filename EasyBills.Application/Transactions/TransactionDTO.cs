namespace EasyBills.Application.Transactions;

/// <summary>
/// Trasaction DTO to render data.
/// </summary>
/// <param name="Id">Transaction id.</param>
/// <param name="Amount">Amount.</param>
/// <param name="Description">Description.</param>
/// <param name="AccountId">Account id.</param>
/// <param name="CategoryId">Category id.</param>
/// <param name="IsIncome">If is an income.</param>
public record TransactionDTO(
    Guid Id,
    decimal Amount,
    string Description,
    Guid AccountId,
    Guid CategoryId,
    bool IsIncome
    );
