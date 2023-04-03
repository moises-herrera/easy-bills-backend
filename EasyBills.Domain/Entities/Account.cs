using EasyBills.Core.Entity;
using EasyBills.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyBills.Domain.Entities
{
    public class Account : Entity
    {
        public string Name { get; set; }
        public FinanceAccountType TypeAccount { get; set; }
        public decimal Balance { get; set; }
    }
}
