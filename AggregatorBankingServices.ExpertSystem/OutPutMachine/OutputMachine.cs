using AggregatorBankingServices.ExpertSystem.Models;
using AggregatorBankingServices.ExpertSystem.OutPutMachine.Interfaces;
using AggregatorBankingServices.ExpertSystem.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AggregatorBankingServices.ExpertSystem.OutPutMachine;
public class OutputMachine
{
    private WorkingMemory _working_memory;
    private IExpertSystemRepositoty _repository;
    public OutputMachine(WorkingMemory working_memory, IExpertSystemRepositoty repository)
    {
        _working_memory = working_memory;
        _repository = repository;
    }

    public async Task<IOutputMachineResponse> GetResponse()
    {
        var triggered_rules = _working_memory.TriggeredRules.Select(tuple => tuple.Rule);
        Rule next_rule = await _repository.GetNextRuleAsync(triggered_rules);
        while (next_rule != null)
        {
            IEnumerable<Rule> chain_of_rules = new List<Rule>();
            chain_of_rules = chain_of_rules.Append(next_rule);
            Rule parent_rule = await _repository.GetParentRuleAsync(next_rule);
            while (parent_rule != null)
            {
                chain_of_rules = chain_of_rules.Prepend(parent_rule);
                parent_rule = await _repository.GetParentRuleAsync(parent_rule);
            }

            Rule child_rule = await _repository.GetChildrenRuleAsync(next_rule);
            while (child_rule != null)
            {
                chain_of_rules = chain_of_rules.Append(child_rule);
                child_rule = await _repository.GetChildrenRuleAsync(child_rule);
            }

            bool chain_is_true = true;

            foreach (var current_rule in chain_of_rules)
            {
                Fact current_rule_fact = current_rule.Fact;
                bool current_fact_is_true = _working_memory.TrueFacts
                    .Any(fact => fact.Id == current_rule_fact.Id);

                if (!current_fact_is_true)
                {
                    Fact? fact = _working_memory.TrueFacts
                        .FirstOrDefault(fact => fact.Variable.Name == current_rule_fact.Variable.Name);
                    if (fact == null)
                    {
                        IEnumerable<DomainValue> domain_values = await _repository.GetDomainValuesAsync(current_rule_fact);
                        IEnumerable<string> options = domain_values.Select(domain_value => domain_value.Name);
                        return new OutputMachineResponse(current_rule_fact.Variable.Question, options, null);
                    }

                    _working_memory.TriggeredRules
                        .AddRange(chain_of_rules.Select(rule => (rule, false)));
                    chain_is_true = false;
                    break;
                }
            }

            if (chain_is_true)
            {
                _working_memory.TriggeredRules
                    .AddRange(chain_of_rules.Select(rule => (rule, true)));
                _working_memory.TrueFacts
                    .Add(chain_of_rules.Last().FactResult);
            }

            next_rule = await _repository.GetNextRuleAsync(triggered_rules);
        }

        double sum_of_points = _working_memory.TrueFacts
            .Where(fact => fact.Variable.Domain.Name == "Баллы")
            .Select(fact => fact.DomainValue.Name)
            .Select(name => double.Parse(name))
            .Sum();

        IEnumerable<Rule> conclusion_rules = await _repository.GetConclusionRulesAsync();
        Rule? triggered_coclusion_rule = conclusion_rules.FirstOrDefault(rule =>
        {
            string[] range_limits = rule.Fact.DomainValue.Name.Replace("От ", "").Replace("до ", "").Split(' ');
            if (range_limits[0] == "x") range_limits[0] = double.MinValue.ToString();
            if (range_limits[1] == "x") range_limits[1] = double.MaxValue.ToString();
            return sum_of_points >= double.Parse(range_limits[0]) && sum_of_points <= double.Parse(range_limits[1]);
        });
        if (triggered_coclusion_rule == null)
        {
            return new OutputMachineResponse(null, null, "Экспертная система не смогла прийти к какому-либо заключению");
        }
        _working_memory.TriggeredRules.Add((triggered_coclusion_rule, true));
        _working_memory.TrueFacts.Add(triggered_coclusion_rule.Fact);
        string conclusion = triggered_coclusion_rule.FactResult.DomainValue.Name;
        return new OutputMachineResponse(null, null, conclusion);
    }
}

