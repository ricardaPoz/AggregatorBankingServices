using System;
using System.Collections.Generic;

namespace AggregatorBankingServices.DataBase.Models
{
    public partial class BankName
    {
        public BankName()
        {
            Contributions = new HashSet<Contribution>();
            Loans = new HashSet<Loan>();
        }

        public string Name { get; set; } = null!;

        public virtual ICollection<Contribution> Contributions { get; set; }
        public virtual ICollection<Loan> Loans { get; set; }
    }
}
