namespace EasyBills.Application.Users;

/// <summary>
/// User DTO used for creation and updates.
/// </summary>ss
/// <param name="FirstName">First name.</param>
/// <param name="LastName">Last name</param>
/// <param name="Email">Email address.</param>
/// <param name="Password">Password.</param>
public record CreateUserDTO(
    string FirstName,
    string LastName,
    string Email,
    string Password);
