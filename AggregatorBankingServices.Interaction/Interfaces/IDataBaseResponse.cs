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
    Task<IEnumerable<Loan>> SearchLoan(BankName bank_name, TypePaymant type_payment, double rate, double loan_amount_from, double loan_amount_to);
}
