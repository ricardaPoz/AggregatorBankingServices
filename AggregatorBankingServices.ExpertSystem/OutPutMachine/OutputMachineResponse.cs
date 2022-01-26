using AggregatorBankingServices.ExpertSystem.OutPutMachine.Interfaces;

namespace AggregatorBankingServices.ExpertSystem.OutPutMachine;
public record OutputMachineResponse(string Question, IEnumerable<string> QuestionOptions, string Conclusion) : IOutputMachineResponse;

