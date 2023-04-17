using EasyBills.Domain.Entities.Enums;

namespace EasyBills.Application.Accounts;

/// <summary>
///  Account DTO for creating or updating data.
/// </summary>
/// <param name="Name">Account name.</param>
/// <param name="TypeAccount">Type of account.</param>
/// <param name="Balance">Balance.</param>
/// <param name="UserId">User id.</param>
public record CreateAccountDTO(
    string Name,
    FinanceAccountType TypeAccount,
    decimal Balance,
    Guid UserId);