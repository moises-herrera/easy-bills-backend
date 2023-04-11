using EasyBills.Core.Entity;

namespace EasyBills.Domain.Entities;

public class Category : Entity
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public List<Transaction> Transactions { get; set; }
}
