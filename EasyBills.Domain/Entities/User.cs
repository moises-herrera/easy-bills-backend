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
    /// Initialize a new instance of <see cref="User"/> class.
    /// </summary>
    /// <param name="firstName">First name.</param>
    /// <param name="lastName">Last name.</param>
    /// <param name="email">Email.</param>
    /// <param name="password">Password.</param>
    public User(string firstName, string lastName, string email, string password)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Password = password;
        IsEmailVerified = false;
        Transactions = new List<Transaction>();
    }

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
    /// Password field.
    /// </summary>
    [NotMapped]
    private string _password = null!;

    /// <summary>
    /// Property used to encrypt the password value.
    /// </summary>
    public string Password
    { 
        get
        {
            return _password;
        } 
        set 
        {
            _password = EncryptionHelper.Encrypt(value);
        }
    }

    /// <summary>
    /// If the email is verified.
    /// </summary>
    public bool IsEmailVerified { get; set; }

    /// <summary>
    /// Transactions.
    /// </summary>
    public List<Transaction> Transactions { get; set; }

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
