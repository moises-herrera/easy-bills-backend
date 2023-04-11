using EasyBills.Domain.Entities;

namespace EasyBills.Application.Users;

public class UserDTO
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public bool IsEmailVerified { get; set; }
    public List<Transaction> Transactions { get; set; }
}

