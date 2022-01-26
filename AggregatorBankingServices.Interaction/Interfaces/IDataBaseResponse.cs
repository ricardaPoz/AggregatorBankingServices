using AggregatorBankingServices.Interaction.Model;
using AggregatorBankingServices.Interaction.ResponseEF;

namespace AggregatorBankingServices.Interaction.Interfaces;
public interface IDataBaseResponse
{
    Task<bool> UserRegistration(User user);
    Task<(bool is_contains, string password)> GetUserNameAndPassword(User user);
    Task<IEnumerable<TypePaymant>> GetTypePayment();
    Task<IEnumerable<Loan>> GetAllLoan();
    Task<IEnumerable<BankName>> GetAllBankNames();
    Task<IEnumerable<Loan>> SearchLoans(BankName bank_name, TypePaymant type_payment, double rate, double loan_amount_from, double loan_amount_to);
    Task<IEnumerable<Contribution>> SearchContribution(BankName bank_name, Capitalization type_capitalization, double rate, double loan_amount_from, double loan_amount_to);
    Task<IEnumerable<Contribution>> GetAllContribution();
    Task<IEnumerable<Capitalization>> GetTypeCapitalization();
}
