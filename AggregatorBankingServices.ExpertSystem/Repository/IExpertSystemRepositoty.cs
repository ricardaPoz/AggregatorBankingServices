using AggregatorBankingServices.ExpertSystem.Interfaces;
using AggregatorBankingServices.ExpertSystem.Models;

namespace AggregatorBankingServices.ExpertSystem.Repository;
 public interface IExpertSystemRepositoty
{
    Task<Rule> GetNextRuleAsync(IEnumerable<Rule> triggered_rules);
    Task<Rule> GetParentRuleAsync(Rule child_rule);
    Task<Rule> GetChildrenRuleAsync(Rule parent_rule);
    Task<IEnumerable<DomainValue>> GetDomainValuesAsync(Fact fact);
    Task<IEnumerable<Rule>> GetConclusionRulesAsync();
    Task<Fact> GetFactByClientAnswerAsync(IClientAnswer answer);
}
