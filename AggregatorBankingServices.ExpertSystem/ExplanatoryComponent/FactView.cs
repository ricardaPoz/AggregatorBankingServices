using AggregatorBankingServices.ExpertSystem.ExplanatoryComponent.Interfaces;

namespace AggregatorBankingServices.ExpertSystem.ExplanatoryComponent;
public record FactView(string VariableName, string VariableValue) : IFact;
