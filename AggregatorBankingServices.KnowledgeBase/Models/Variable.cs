using System;
using System.Collections.Generic;

namespace AggregatorBankingServices.KnowledgeBase.Models
{
    public partial class Variable
    {
        public Variable()
        {
            Facts = new HashSet<Fact>();
        }

        public string Name { get; set; } = null!;
        public string DomainName { get; set; } = null!;
        public string VariableTypeName { get; set; } = null!;
        public string? Question { get; set; }

        public virtual Domain DomainNameNavigation { get; set; } = null!;
        public virtual VariablesType VariableTypeNameNavigation { get; set; } = null!;
        public virtual ICollection<Fact> Facts { get; set; }
    }
}
