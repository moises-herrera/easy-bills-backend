using EasyBills.Domain.Entities;

namespace EasyBills.Application.Categories;

/// <summary>
/// Category DTO for render data.
/// </summary>
/// <param name="Id">Id.</param>
/// <param name="Name">Category name.</param>
/// <param name="Description">Category description.</param>
/// <param name="UserId">User id.</param>
/// <param name="Transactions">Transactions.</param>
public record CategoryDTO(
    Guid Id,
    string Name,
    string? Description,
    Guid? UserId,
    List<Transaction> Transactions
    );
