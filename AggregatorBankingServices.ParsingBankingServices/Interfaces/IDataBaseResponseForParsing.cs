using AggregatorBankingServices.Interaction.Model;

namespace AggregatorBankingServices.ParsingBankingServices.Interfaces;
public interface IDataBaseResponseForParsing 
{
    Task AddBankName(BankName bank_name);
    Task AddContribution(Contribution contribution);
    Task AddLoan(Loan loan);
    Task<bool> IsContainsBankName(BankName bank_name);
    Task<bool> IsContainsContribution(Contribution contribution);
    Task<bool> IsContainsLoan (Loan loan);
}
