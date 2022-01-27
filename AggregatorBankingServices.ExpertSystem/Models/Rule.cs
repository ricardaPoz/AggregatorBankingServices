namespace AggregatorBankingServices.ExpertSystem.Models;
public class Rule
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Fact Fact { get; set; }
    public Fact FactResult { get; set; }
    public int? AdditionalRuleId { get; set; }
    public string? Description { get; set; }
}
