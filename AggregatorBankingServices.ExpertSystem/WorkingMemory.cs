using AggregatorBankingServices.ExpertSystem.Models;

namespace AggregatorBankingServices.ExpertSystem;
public class WorkingMemory
{
    public List<(Rule Rule, bool IsTue)> TriggeredRules { get; init; } = new List<(Rule Rule, bool IsTue)>();
    public List<Fact> TrueFacts { get; } = new List<Fact>();
}
