
using AggregatorBankingServices.ExpertSystem;
using AggregatorBankingServices.ExpertSystem.Interfaces;
using AggregatorBankingServices.ExpertSystem.Repository;
using AggregatorBankingServices.KnowledgeBase;
using ExpertSystemTest;

IExpertSystemRepositoty repository = new EFKnowledgeBase();
ExpertSystem expert_system = new ExpertSystem(repository);
IExpertSystemResponse response = expert_system.GetResponse();
while (response.OutputMachineResponse.Conclusion == null)
{
    var question = response.OutputMachineResponse.Question;
    Console.WriteLine(question);
    var options = response.OutputMachineResponse.QuestionOptions.ToList();
    for (int i = 0; i < options.Count; i++)
    {
        Console.WriteLine($"{i} - {options[i]}");
    }
    var answer_index = int.Parse(Console.ReadLine());
    var answer = new ClientAnswer(question, options[answer_index]);
    response = expert_system.GetResponse(answer);
}

Console.WriteLine($"Заключение: {response.OutputMachineResponse.Conclusion}");

Console.WriteLine("Объяснение: ");

foreach (var explanation in response.ExplanatoryMachineResponse.Explanations)
{
    Console.WriteLine($"Правило: {explanation.RuleName}");
    Console.WriteLine($"\tЕсли:");
    foreach (var fact in explanation.TrueFacts)
    {
        Console.WriteLine($"\t\t{fact.VariableName} = {fact.VariableValue}");
    }
    Console.WriteLine($"\tТо:");
    Console.WriteLine($"\t\t{explanation.ResultingFact.VariableName} = {explanation.ResultingFact.VariableValue}");
    Console.WriteLine("\tТак как:");
    Console.WriteLine($"\t\t{explanation.ExplanationText}");
}
