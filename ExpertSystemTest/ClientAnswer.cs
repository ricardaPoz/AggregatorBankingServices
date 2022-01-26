using AggregatorBankingServices.ExpertSystem.Interfaces;

namespace ExpertSystemTest;
public record ClientAnswer(string Question, string SelectedAnswer) : IClientAnswer;

