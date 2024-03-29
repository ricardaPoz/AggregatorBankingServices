﻿using System;
using System.Collections.Generic;

namespace AggregatorBankingServices.KnowledgeBase.Models
{
    public partial class Rule
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int FactId { get; set; }
        public int? FactResultId { get; set; }
        public int? AdditionalRuleId { get; set; }
        public string? Description { get; set; }

        public virtual Fact Fact { get; set; } = null!;
        public virtual Fact? FactResult { get; set; }
    }
}
