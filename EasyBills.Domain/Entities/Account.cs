using EasyBills.Core.Entity;
using EasyBills.Domain.Entities.Enums;

namespace EasyBills.Domain.Entities;

public class Account : Entity
{
    public string Name { get; set; }
    public FinanceAccountType TypeAccount { get; set; }
    public decimal Balance { get; set; }
    public List<Transaction> Transactions { get; set; }
}
