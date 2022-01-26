namespace AggregatorBankingServices.ExpertSystem.Models;
public class Variable
{
    public string Name { get; set; }
    public Domain Domain { get; set; }
    public VariableType VariableType { get; set; }
    public string? Question { get; set; }
}

