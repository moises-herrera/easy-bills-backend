namespace EasyBills.Application.Categories;

/// <summary>
/// Category DTO used for creation and updates.
/// </summary>
/// <param name="Name">Category name.</param>
/// <param name="Icon">Category icon.</param>
/// <param name="Color">Category color.</param>
/// <param name="UserId">User id.</param>
/// <param name="Description">Category description.</param>
public record CreateCategoryDTO(
    string Name,
    string Icon,
    string Color,
    Guid? UserId,
    string? Description
    );
