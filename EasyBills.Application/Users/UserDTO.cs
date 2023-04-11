using EasyBills.Domain.Entities;

namespace EasyBills.Application.Users;

/// <summary>
/// User DTO for render data.
/// </summary>
/// <param name="Id">Id.</param>
/// <param name="FirstName">First name.</param>
/// <param name="LasName">Last name.</param>
/// <param name="Email">Email address.</param>
/// <param name="Password">Password.</param>
/// <param name="IsEmailVerified">If the email is verified.</param>
/// <param name="Transactions">Transactions.</param>
public record UserDTO(
    Guid Id, 
    string FirstName, 
    string LasName, 
    string Email, string 
    Password, 
    bool IsEmailVerified, 
    List<Transaction> Transactions);