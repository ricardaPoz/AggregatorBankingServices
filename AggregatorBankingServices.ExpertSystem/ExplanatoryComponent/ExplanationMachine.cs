using AggregatorBankingServices.ExpertSystem.ExplanatoryComponent.Interfaces;
using AggregatorBankingServices.ExpertSystem.Models;

namespace AggregatorBankingServices.ExpertSystem.ExplanatoryComponent;
public class ExplanationMachine
{
    private WorkingMemory _working_memory;
    public ExplanationMachine(WorkingMemory working_memory)
    {
        _working_memory = working_memory;
    }
    public IExplanationComponentResponse GetResponse()
    {
        IEnumerable<Rule> triggered_true_rule = _working_memory.TriggeredRules
            .Where(tuple => tuple.IsTue)
            .Select(tuple => tuple.Rule);

        IEnumerable<IExplanation> explanations = triggered_true_rule
            .Where(current_rule => triggered_true_rule
                .All(other_rule => other_rule.AdditionalRule?.Name != current_rule.Name))
            .Select(first_rule =>
            {
                List<Rule> chain_of_rules = new List<Rule>();
                Rule current_rule = first_rule;
                while (current_rule != null)
                {
                    chain_of_rules.Add(current_rule);
                    current_rule = current_rule.AdditionalRule;
                }
                Rule last_rule = chain_of_rules.Last();
                string last_rule_name = last_rule.Name;
                IEnumerable<IFact> facts = chain_of_rules
                    .Select(rule => new FactView(rule.Fact.Variable.Name, rule.Fact.DomainValue.Name));
                string last_rule_explanation = last_rule.Description;
                IFact resulting_fact = new FactView(last_rule.FactResult.Variable.Name, last_rule.FactResult.DomainValue.Name);
                return new Explanation(last_rule_name, facts, resulting_fact, last_rule_explanation);
            });

        return new ExplanationComponentResponse(explanations);
    }
}
