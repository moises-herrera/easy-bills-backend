using EasyBills.Core.Entity;
using EasyBills.Security.Helpers;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyBills.Domain.Entities;

/// <summary>
/// Represents the user entity.
/// </summary>
public class User : Entity
{
    /// <summary>
    /// First name.
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// Last name.
    /// </summary>
    public string LastName { get; set; }

    /// <summary>
    /// Email.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Property used to encrypt the password value.
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// If the email is verified.
    /// </summary>
    public bool IsEmailVerified { get; set; }

    /// <summary>
    /// Finance accounts.
    /// </summary>
    public List<Account> Accounts { get; set; }

    /// <summary>
    /// Custom categories.
    /// </summary>
    public List<Category>? Categories { get; set; }

    /// <summary>
    /// Full name.
    /// </summary>
    [NotMapped]
    public string FullName => $"{FirstName} {LastName}";

    /// <summary>
    /// Validate if the password passed as parameter is equal to the current password.
    /// </summary>
    /// <param name="password">The password input.</param>
    /// <returns>If the passwords match.</returns>
    public bool IsValidPassword(string password) => 
        Password == EncryptionHelper.Encrypt(password);
}
