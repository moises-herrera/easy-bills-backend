namespace EasyBills.Application.Users;

/// <summary>
/// DTO for the user login.
/// </summary>
/// <param name="Email">User email</param>
/// <param name="Password">User password.</param>
public record LoginUserDTO(
    string Email, 
    string Password);