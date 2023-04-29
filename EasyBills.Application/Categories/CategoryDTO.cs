namespace EasyBills.Application.Categories;

/// <summary>
/// Category DTO for render data.
/// </summary>
/// <param name="Id">Id.</param>
/// <param name="Name">Category name.</param>
/// <param name="Icon">Category icon.</param>
/// <param name="Icon">Category color.</param>
/// <param name="Description">Category description.</param>
/// <param name="UserId">User id.</param>
public record CategoryDTO(
    Guid Id,
    string Name,
    string Icon,
    string Color,
    string? Description,
    Guid? UserId
    );
