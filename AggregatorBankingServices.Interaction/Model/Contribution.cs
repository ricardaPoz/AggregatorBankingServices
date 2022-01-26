using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AggregatorBankingServices.Interaction.Model;
public class Contribution
{
    public string Name { get; set; } = null!;
    public decimal Rate { get; set; }
    public decimal DepositAmountFrom { get; set; }
    public decimal DepositAmountTo { get; set; }
    public long TermFrom { get; set; }
    public long TermTo { get; set; }
    public string? Capitalization { get; set; }
    public string? PaymentInterest { get; set; }
    public string? Replenishment { get; set; }
    public string? PartialRemoval { get; set; }
    public string NameBank { get; set; } = null!;
}
