using EasyBills.Domain.Entities.Enums;

namespace EasyBills.Application.Accounts;

/// <summary>
///  Account DTO to render data.
/// </summary>
/// <param name="Id">Id.</param>
/// <param name="Name">Account name.</param>
/// <param name="TypeAccount">Type of account.</param>
/// <param name="Balance">Balance.</param>
/// <param name="UserId">User id.</param>
public record AccountDTO(
    Guid Id,
    string Name,
    FinanceAccountType TypeAccount,
    decimal Balance,
    Guid UserId);
