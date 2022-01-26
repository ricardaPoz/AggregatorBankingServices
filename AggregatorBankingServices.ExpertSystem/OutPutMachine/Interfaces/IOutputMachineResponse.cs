namespace AggregatorBankingServices.ExpertSystem.OutPutMachine.Interfaces;
public interface IOutputMachineResponse
{
    string Question { get; init; }
    IEnumerable<string> QuestionOptions { get; init; }
    string Conclusion { get; init; }
}
