namespace EasyBills.Application.Categories;

/// <summary>
/// Category DTO used for creation and updates.
/// </summary>
/// <param name="Name">Category name.</param>
/// <param name="UserId">User id.</param>
/// <param name="Description">Category description.</param>
public record CreateCategoryDTO(
    string Name,
    Guid? UserId,
    string? Description
    );
