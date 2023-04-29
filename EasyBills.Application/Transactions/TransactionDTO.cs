using EasyBills.Application.Accounts;
using EasyBills.Application.Categories;
using EasyBills.Domain.Entities.Enums;

namespace EasyBills.Application.Transactions;

/// <summary>
/// Trasaction DTO to render data.
/// </summary>
/// <param name="Id">Transaction id.</param>
/// <param name="Amount">Amount.</param>
/// <param name="Description">Description.</param>
/// <param name="Account">Account.</param>
/// <param name="Category">Category.</param>
/// <param name="TransactionType">Type of the transaction.</param>
public record TransactionDTO(
    Guid Id,
    decimal Amount,
    string Description,
    AccountDTO Account,
    CategoryDTO Category,
    TransactionType TransactionType
    );
