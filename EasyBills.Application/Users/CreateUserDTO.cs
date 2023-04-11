namespace EasyBills.Application.Users;

public record CreateUserDTO(
    string FirstName,
    string LastName,
    string Email,
    string Password);
