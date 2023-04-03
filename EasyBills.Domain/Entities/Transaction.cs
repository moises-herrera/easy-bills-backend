using EasyBills.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyBills.Domain.Entities
{
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
}
