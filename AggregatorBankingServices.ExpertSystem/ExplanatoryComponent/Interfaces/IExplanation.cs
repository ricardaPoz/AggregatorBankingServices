namespace AggregatorBankingServices.ExpertSystem.ExplanatoryComponent.Interfaces;
public interface IExplanation
{
    public string RuleName { get; init; }
    public IEnumerable<IFact> TrueFacts { get; init; }
    public IFact ResultingFact { get; init; }
    public string ExplanationText { get; init; }
}
