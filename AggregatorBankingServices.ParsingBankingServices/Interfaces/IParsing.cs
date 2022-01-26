using AggregatorBankingServices.Interaction.Model;
namespace AggregatorBankingServices.ParsingBankingServices.Interfaces;
public interface IParsing
{
    Dictionary<BankName, List<string>> GetProducts(string selector);
    Task Pars(BankName name, string link);
    void GetDescriptionAsync(Dictionary<BankName, List<string>> all_product);
}
