using AggregatorBankingServices.ExpertSystem.Repository;

using RuleExpert = AggregatorBankingServices.ExpertSystem.Models.Rule;
using RuleData = AggregatorBankingServices.KnowledgeBase.Models.Rule;
using FactExpert = AggregatorBankingServices.ExpertSystem.Models.Fact;
using FactData = AggregatorBankingServices.KnowledgeBase.Models.Fact;
using VariableExpert = AggregatorBankingServices.ExpertSystem.Models.Variable;
using VariableData = AggregatorBankingServices.KnowledgeBase.Models.Variable;
using DomainValueExpert = AggregatorBankingServices.ExpertSystem.Models.DomainValue;
using DomainValueData = AggregatorBankingServices.KnowledgeBase.Models.DomainValue;
using VariableTypeExpert = AggregatorBankingServices.ExpertSystem.Models.VariableType;
using VariableTypeData = AggregatorBankingServices.KnowledgeBase.Models.VariablesType;
using DomainExpert = AggregatorBankingServices.ExpertSystem.Models.Domain;
using DomainData = AggregatorBankingServices.KnowledgeBase.Models.Domain;
using AutoMapper;
using AggregatorBankingServices.ExpertSystem.Interfaces;
using AggregatorBankingServices.KnowledgeBase.Models;
using Microsoft.EntityFrameworkCore;

namespace AggregatorBankingServices.KnowledgeBase;
public class EFKnowledgeBase : IExpertSystemRepositoty, IExperSystemCRUT
{
    private readonly KnowledgeBaseContext _context = new KnowledgeBaseContext();
    private readonly IMapper _mapper;
    private readonly object locker = new object();
    public EFKnowledgeBase()
    {
        var config = new MapperConfiguration(conf =>
        {
            conf.CreateMap<RuleData, RuleExpert>();

            conf.CreateMap<FactData, FactExpert>()
                .ForMember(fact_expert => fact_expert.Variable,
                conf => conf.MapFrom(fact_data => fact_data.VariableNameNavigation))
                .ForMember(fact_expert => fact_expert.DomainValue,
                conf => conf.MapFrom(fact_data => fact_data.DomainValueNameNavigation));

            conf.CreateMap<VariableData, VariableExpert>()
                .ForMember(variable_expert => variable_expert.Domain,
                conf => conf.MapFrom(variable_data => variable_data.DomainNameNavigation))
                .ForMember(variable_expert => variable_expert.VariableType,
                conf => conf.MapFrom(variable_data => variable_data.VariableTypeNameNavigation));

            conf.CreateMap<VariableTypeData, VariableTypeExpert>();

            conf.CreateMap<DomainValueData, DomainValueExpert>()
                .ForMember(domain_value_expert => domain_value_expert.Domain,
                conf => conf.MapFrom(domain_value_data => domain_value_data.DomainNameNavigation));

            conf.CreateMap<DomainData, DomainExpert>();
        });
        _mapper = config.CreateMapper();
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    public async Task<RuleExpert> GetNextRuleAsync(IEnumerable<RuleExpert> triggered_rules)
    {
        var id_triggered_rules = triggered_rules.Select(triggered_rule => triggered_rule.Id);
        RuleData? next_rule = _context.Rules
            .Include(rule => rule.Fact.VariableNameNavigation.VariableTypeNameNavigation)
            .Include(rule => rule.Fact.DomainValueNameNavigation.DomainNameNavigation)
            .Include(rule => rule.FactResult.VariableNameNavigation.VariableTypeNameNavigation)
            .Include(rule => rule.FactResult.DomainValueNameNavigation.DomainNameNavigation)
            .Where(rule => !id_triggered_rules.Contains(rule.Id))
            .Where(rule => rule.Fact.VariableNameNavigation.VariableTypeName != "Вычисляемая")
            .FirstOrDefault();
        return _mapper.Map<RuleExpert>(next_rule);
    }

    public async Task<RuleExpert> GetParentRuleAsync(RuleExpert child_rule)
    {
        RuleData? parent_rule = _context.Rules
            .Include(rule => rule.Fact.VariableNameNavigation.VariableTypeNameNavigation)
            .Include(rule => rule.Fact.DomainValueNameNavigation.DomainNameNavigation)
            .Include(rule => rule.FactResult.VariableNameNavigation.VariableTypeNameNavigation)
            .Include(rule => rule.FactResult.DomainValueNameNavigation.DomainNameNavigation)
            .Where(rule => rule.AdditionalRuleId == child_rule.Id)
            .FirstOrDefault();
        return _mapper.Map<RuleExpert>(parent_rule);
    }

    public async Task<RuleExpert> GetChildrenRuleAsync(RuleExpert parent_rule)
    {
        if (parent_rule.AdditionalRuleId == null) return null;

        RuleData? children_rule = _context.Rules
            .Include(rule => rule.Fact.VariableNameNavigation.VariableTypeNameNavigation)
            .Include(rule => rule.Fact.DomainValueNameNavigation.DomainNameNavigation)
            .Include(rule => rule.FactResult.VariableNameNavigation.VariableTypeNameNavigation)
            .Include(rule => rule.FactResult.DomainValueNameNavigation.DomainNameNavigation)
            .Where(rule => parent_rule.AdditionalRuleId == rule.Id)
            .FirstOrDefault();
        return _mapper.Map<RuleExpert>(children_rule);
    }

    public async Task<IEnumerable<DomainValueExpert>> GetDomainValuesAsync(FactExpert fact)
    {
        List<DomainValueData>? domain_values = _context.DomainValues
            .Where(domain_value => domain_value.DomainName == fact.DomainValue.Domain.Name)
            .ToList();
        return _mapper.ProjectTo<DomainValueExpert>(domain_values.AsQueryable());
    }

    public async Task<IEnumerable<RuleExpert>> GetConclusionRulesAsync()
    {
        List<RuleData>? conslusion_rules = _context.Rules
            .Include(rule => rule.Fact.VariableNameNavigation.VariableTypeNameNavigation)
            .Include(rule => rule.Fact.DomainValueNameNavigation.DomainNameNavigation)
            .Include(rule => rule.FactResult.VariableNameNavigation.VariableTypeNameNavigation)
            .Include(rule => rule.FactResult.DomainValueNameNavigation.DomainNameNavigation)
            .Where(rule => rule.Fact.VariableNameNavigation.VariableTypeNameNavigation.Name == "Вычисляемая")
            .ToList();
        return _mapper.ProjectTo<RuleExpert>(conslusion_rules.AsQueryable());
    }

    public async Task<FactExpert> GetFactByClientAnswerAsync(IClientAnswer answer)
    {
        FactData? client_fact =  _context.Facts
            .Include(fact => fact.VariableNameNavigation.VariableTypeNameNavigation)
            .Include(fact => fact.DomainValueNameNavigation)
            .Include(fact => fact.DomainValueNameNavigation.DomainNameNavigation)
            .Where(fact => fact.VariableNameNavigation.Question == answer.Question)
            .Where(fact => fact.DomainValueNameNavigation.Name == answer.SelectedAnswer)
            .FirstOrDefault();
        return _mapper.Map<FactExpert>(client_fact);
    }

    public async Task<IEnumerable<DomainExpert>> SelectDomainAsync()
    {
        lock (locker)
        {
            var domains = _context.Domains.ToList();
            return _mapper.ProjectTo<DomainExpert>(domains.AsQueryable());
        }
    }

    public async Task<IEnumerable<DomainValueExpert>> SelectDomainValuesAsync()
    {
        lock (locker)
        {
            var domains = _context.DomainValues.ToList();
            return _mapper.ProjectTo<DomainValueExpert>(domains.AsQueryable());
        }
    }
    public async Task<IEnumerable<VariableTypeExpert>> SelectVaribleTypesAsync()
    {
        lock (locker)
        {
            var domains = _context.VariablesTypes.ToList();
            return _mapper.ProjectTo<VariableTypeExpert>(domains.AsQueryable());
        }
    }

    public async Task<IEnumerable<FactExpert>> SelectFactsAsync()
    {
        lock (locker)
        {
            var facts = _context.Facts
                .Include(fact => fact.VariableNameNavigation);
            return _mapper.ProjectTo<FactExpert>(facts.AsQueryable());
        }
    }

    public async Task<IEnumerable<VariableExpert>> SelectVariablesAsync()
    {
        lock (locker)
        {
            var facts = _context.Variables
                .Include(variable => variable.DomainNameNavigation)
                .Include(variable => variable.VariableTypeNameNavigation);
            return _mapper.ProjectTo<VariableExpert>(facts.AsQueryable());
        }
    }

    public async Task<IEnumerable<RuleExpert>> SelectRulesAsync()
    {
        lock (locker)
        {
            var rules = _context.Rules
                .Include(rule => rule.Fact)
                .ToList();
            return _mapper.ProjectTo<RuleExpert>(rules.AsQueryable());
        }
    }
}
