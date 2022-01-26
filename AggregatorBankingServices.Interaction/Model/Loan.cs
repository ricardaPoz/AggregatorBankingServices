namespace AggregatorBankingServices.Interaction.Model;
public record class Loan(string Name, decimal Rate, decimal LoanAmountFrom, decimal LoanAmountTo, string TypePayment,
    string PaymentFrequency, string ApplicationReview, string MandatoryInsurance, string RequiredDocuments, string IncomeVerification,
    long AgeRedemptionMan, long AgeRedemptionWomen, long BorrowerAge, long TermFrom, long? TermTo, string NameBank);

