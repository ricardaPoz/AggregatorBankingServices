using AggregatorBankingServices.ExpertSystem.Models;

namespace AggregatorBankingServices.ExpertSystem.Repository;
public interface IExperSystemCRUT
{
    Task<IEnumerable<Domain>> SelectDomainAsync();
    Task<IEnumerable<DomainValue>> SelectDomainValuesAsync();
    Task<IEnumerable<VariableType>> SelectVaribleTypesAsync();
    Task<IEnumerable<Fact>> SelectFactsAsync();
    Task<IEnumerable<Variable>> SelectVariablesAsync();
}
