using AggregatorBankingServices.ExpertSystem.ExplanatoryComponent.Interfaces;
using AggregatorBankingServices.ExpertSystem.Interfaces;
using AggregatorBankingServices.ExpertSystem.Models;
using AggregatorBankingServices.ExpertSystem.OutPutMachine;
using AggregatorBankingServices.ExpertSystem.OutPutMachine.Interfaces;
using AggregatorBankingServices.ExpertSystem.Repository;
using ExplanationMachine = AggregatorBankingServices.ExpertSystem.ExplanatoryComponent.ExplanationMachine;

namespace AggregatorBankingServices.ExpertSystem;
public class ExpertSystem
{
    private OutputMachine _output_machine;
    private ExplanationMachine _explanatory_component;
    private WorkingMemory _working_memory;
    private IExpertSystemRepositoty _repository;

    public ExpertSystem(IExpertSystemRepositoty repository)
    {
        _working_memory = new WorkingMemory();
        _repository = repository;
        _output_machine = new OutputMachine(_working_memory, _repository);
        _explanatory_component = new ExplanationMachine(repository, _working_memory);
    }
    public IExpertSystemResponse GetResponse(IClientAnswer answer)
    {
        Fact client_fact = _repository.GetFactByClientAnswerAsync(answer).Result;
        _working_memory.TrueFacts.Add(client_fact);

        IOutputMachineResponse output_machine_response = _output_machine.GetResponse().Result;
        IExplanationComponentResponse explanatory_machine_response = null;
        if (output_machine_response.Conclusion != null) explanatory_machine_response = _explanatory_component.GetResponse().Result;
        return new ExpertSystemResponse(output_machine_response, explanatory_machine_response);
    }

    public IExpertSystemResponse GetResponse()
    {
        IOutputMachineResponse output_machine_response = _output_machine.GetResponse().Result;
        IExplanationComponentResponse explanatory_machine_response = null;
        if (output_machine_response.Conclusion != null) explanatory_machine_response = _explanatory_component.GetResponse().Result;
        return new ExpertSystemResponse(output_machine_response, explanatory_machine_response);
    }
}
