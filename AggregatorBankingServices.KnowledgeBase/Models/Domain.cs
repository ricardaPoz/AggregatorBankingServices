using System;
using System.Collections.Generic;

namespace AggregatorBankingServices.KnowledgeBase.Models
{
    public partial class Domain
    {
        public Domain()
        {
            DomainValues = new HashSet<DomainValue>();
            Variables = new HashSet<Variable>();
        }

        public string Name { get; set; } = null!;

        public virtual ICollection<DomainValue> DomainValues { get; set; }
        public virtual ICollection<Variable> Variables { get; set; }
    }
}
