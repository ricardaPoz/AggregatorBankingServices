using AggregatorBankingServices.ExpertSystem.ExplanatoryComponent.Interfaces;

namespace AggregatorBankingServices.ExpertSystem.ExplanatoryComponent;
public record Explanation(string RuleName, IEnumerable<IFact> TrueFacts, IFact ResultingFact, string ExplanationText) : IExplanation;

