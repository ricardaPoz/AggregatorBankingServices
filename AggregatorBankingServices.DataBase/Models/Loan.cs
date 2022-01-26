using System;
using System.Collections.Generic;

namespace AggregatorBankingServices.DataBase.Models
{
    public partial class Loan
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal? Rate { get; set; }
        public decimal? LoanAmountFrom { get; set; }
        public decimal? LoanAmountTo { get; set; }
        public string? TypePayment { get; set; }
        public string? PaymentFrequency { get; set; }
        public string? ApplicationReview { get; set; }
        public string? MandatoryInsurance { get; set; }
        public string? RequiredDocuments { get; set; }
        public string? IncomeVerification { get; set; }
        public long? AgeRedemptionMan { get; set; }
        public long? AgeRedemptionWomen { get; set; }
        public long? BorrowerAge { get; set; }
        public long? TermFrom { get; set; }
        public long? TermTo { get; set; }
        public string NameBank { get; set; } = null!;

        public virtual BankName NameBankNavigation { get; set; } = null!;
    }
}
