using EasyBills.Core.Entity;

namespace EasyBills.Domain.Entities;

public class Transaction : Entity
{
    public decimal Amount { get; set; }
    public string Description { get; set; }
    public Guid UserId { get; set; }
    public Guid AccountId { get; set; }
    public Guid CategoryId { get; set; }
    public DateTime CreatedDate { get; set; }
    public bool IsIncome { get; set; }
}
