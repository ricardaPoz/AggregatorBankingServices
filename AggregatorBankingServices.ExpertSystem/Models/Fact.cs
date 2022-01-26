namespace AggregatorBankingServices.ExpertSystem.Models;
public class Fact
{
    public int Id { get; set; }
    public Variable Variable { get; set; }
    public DomainValue DomainValue { get; set; }
}

