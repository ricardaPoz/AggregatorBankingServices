using System;
using System.Collections.Generic;

namespace AggregatorBankingServices.DataBase.Models
{
    public partial class Contribution
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Rate { get; set; }
        public decimal DepositAmountFrom { get; set; }
        public decimal DepositAmountTo { get; set; }
        public long TermFrom { get; set; }
        public long TermTo { get; set; }
        public string Capitalization { get; set; } = null!;
        public string? PaymentInterest { get; set; }
        public string Replenishment { get; set; } = null!;
        public string PartialRemoval { get; set; } = null!;
        public string NameBank { get; set; } = null!;

        public virtual BankName NameBankNavigation { get; set; } = null!;
    }
}
