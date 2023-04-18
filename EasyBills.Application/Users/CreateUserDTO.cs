using EasyBills.Domain.Entities.Enums;

namespace EasyBills.Application.Users;

/// <summary>
/// User DTO used for creation and updates.
/// </summary>ss
/// <param name="FirstName">First name.</param>
/// <param name="LastName">Last name</param>
/// <param name="Email">Email address.</param>
/// <param name="Password">Password.</param>
/// <param name="IsEmailVerified">If the email is verified.</param>
/// <param name="Role">Role of the user.</param>
public record CreateUserDTO(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    bool IsEmailVerified,
    UserRole Role);
