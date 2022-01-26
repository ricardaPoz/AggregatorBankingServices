namespace AggregatorBankingServices.Interaction.Model;
public record class Contribution(string Name, decimal Rate, decimal DepositAmountFrom, decimal DepositAmountTo,
    long TermFrom, long TermTo, string Capitalization, string? PaymentInterest, string Replenishment,
    string PartialRemoval, string NameBank);
