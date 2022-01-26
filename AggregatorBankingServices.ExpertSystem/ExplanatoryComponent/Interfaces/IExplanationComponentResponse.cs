namespace AggregatorBankingServices.ExpertSystem.ExplanatoryComponent.Interfaces;
public interface IExplanationComponentResponse
{
    IEnumerable<IExplanation> Explanations { get; init; }
}
