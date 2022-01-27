using AggregatorBankingServices.ExpertSystem.ExplanatoryComponent.Interfaces;
using AggregatorBankingServices.ExpertSystem.Models;
using AggregatorBankingServices.ExpertSystem.Repository;


namespace AggregatorBankingServices.ExpertSystem.ExplanatoryComponent;
public class ExplanationMachine
{
    private WorkingMemory _working_memory;
    IExpertSystemRepositoty _repository;
    public ExplanationMachine(IExpertSystemRepositoty repository, WorkingMemory working_memory)
    {
        _working_memory = working_memory;
        _repository = repository;

    }
    public async Task<IExplanationComponentResponse> GetResponse()
    {
        IEnumerable<Rule> triggered_true_rule = _working_memory.TriggeredRules
            .Where(tuple => tuple.IsTue)
            .Select(tuple => tuple.Rule);

        IEnumerable<Rule> first_rules = triggered_true_rule
            .Where(current_rule => triggered_true_rule
                .All(other_rule => other_rule.AdditionalRuleId != current_rule.Id));

        List<Explanation> explanations = new List<Explanation>();

        foreach (var first_rule in first_rules)
        {
            IEnumerable<Rule> chain_of_rules = new List<Rule>();
            chain_of_rules = chain_of_rules.Append(first_rule);

            Rule child_rule = await _repository.GetChildrenRuleAsync(first_rule);
            while (child_rule != null)
            {
                chain_of_rules = chain_of_rules.Append(child_rule);
                child_rule = await _repository.GetChildrenRuleAsync(child_rule);
            }
            Rule last_rule = chain_of_rules.Last();
            string last_rule_name = last_rule.Name;
            IEnumerable<IFact> facts = chain_of_rules
                .Select(rule => new FactView(rule.Fact.Variable.Name, rule.Fact.DomainValue.Name));
            string last_rule_explanation = last_rule.Description;
            IFact resulting_fact = new FactView(last_rule.FactResult.Variable.Name, last_rule.FactResult.DomainValue.Name);
            explanations.Add(new Explanation(last_rule_name, facts, resulting_fact, last_rule_explanation));
        }

        return new ExplanationComponentResponse(explanations);
    }
}
