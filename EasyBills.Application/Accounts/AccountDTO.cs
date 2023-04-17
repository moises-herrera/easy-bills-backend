﻿using EasyBills.Domain.Entities;
using EasyBills.Domain.Entities.Enums;

namespace EasyBills.Application.Accounts;

/// <summary>
///  Account DTO to render data.
/// </summary>
/// <param name="Id">Id.</param>
/// <param name="TypeAccount">Type of account.</param>
/// <param name="Balance">Balance.</param>
/// <param name="UserId">User id.</param>
/// <param name="Transactions">Transactions.</param>
public record AccountDTO(
    Guid Id, 
    FinanceAccountType TypeAccount,
    decimal Balance,
    Guid UserId,
    List<Transaction> Transactions);