using AggregatorBankingServices.ExpertSystem.ExplanatoryComponent.Interfaces;
using AggregatorBankingServices.ExpertSystem.OutPutMachine.Interfaces;

namespace AggregatorBankingServices.ExpertSystem.Interfaces;
public interface IExpertSystemResponse
{
    IOutputMachineResponse OutputMachineResponse { get; init; }
    IExplanationComponentResponse ExplanatoryMachineResponse { get; init; }
}
