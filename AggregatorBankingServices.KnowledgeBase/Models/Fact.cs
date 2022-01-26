using System;
using System.Collections.Generic;

namespace AggregatorBankingServices.KnowledgeBase.Models
{
    public partial class Fact
    {
        public Fact()
        {
            RuleFactResults = new HashSet<Rule>();
            RuleFacts = new HashSet<Rule>();
        }

        public int Id { get; set; }
        public string VariableName { get; set; } = null!;
        public string DomainValueName { get; set; } = null!;

        public virtual DomainValue DomainValueNameNavigation { get; set; } = null!;
        public virtual Variable VariableNameNavigation { get; set; } = null!;
        public virtual ICollection<Rule> RuleFactResults { get; set; }
        public virtual ICollection<Rule> RuleFacts { get; set; }
    }
}
