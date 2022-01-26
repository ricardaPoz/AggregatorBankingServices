using System;
using System.Collections.Generic;

namespace AggregatorBankingServices.KnowledgeBase.Models
{
    public partial class VariablesType
    {
        public VariablesType()
        {
            Variables = new HashSet<Variable>();
        }

        public string Name { get; set; } = null!;

        public virtual ICollection<Variable> Variables { get; set; }
    }
}
