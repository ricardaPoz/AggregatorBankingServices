using System;
using System.Collections.Generic;

namespace AggregatorBankingServices.KnowledgeBase.Models
{
    public partial class DomainValue
    {
        public DomainValue()
        {
            Facts = new HashSet<Fact>();
        }

        public string Name { get; set; } = null!;
        public string DomainName { get; set; } = null!;

        public virtual Domain DomainNameNavigation { get; set; } = null!;
        public virtual ICollection<Fact> Facts { get; set; }
    }
}
