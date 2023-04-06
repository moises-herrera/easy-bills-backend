using EasyBills.Core.Entity;
using EasyBills.Security.Helpers;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyBills.Domain.Entities;

public class User : Entity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }

    [NotMapped]
    private string _password;

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
    public bool IsEmailVerified { get; set; }
    public List<Transaction> Transactions { get; set; }

    [NotMapped]
    public string FullName => $"{FirstName} {LastName}";
    public bool IsValidPassword(string password) => 
        Password == EncryptionHelper.Encrypt(password);
}
