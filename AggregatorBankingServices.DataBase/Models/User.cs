using System;
using System.Collections.Generic;

namespace AggregatorBankingServices.DataBase.Models;
public partial class User
{
    public string Login { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string? Scoring { get; set; }
}
