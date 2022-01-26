using AggregatorBankingServices.Interaction.Model;
using AggregatorBankingServices.ParsingBankingServices.Interfaces;
using AngleSharp;
using AngleSharp.Html.Dom;

namespace AggregatorBankingServices.ParsingBankingServices;
public class ParsingContribution
{
    const string Reference = "https://www.sravni.ru/vklady/vse-produkty/";
    const double DaysOfYear = 365.5;
    const double DaysOfMonth = 30.417;
    const string Selector = "div.o084hb-2.hEaIfF";

    private IDataBaseResponseForParsing repository;
    public ParsingContribution(IDataBaseResponseForParsing repository)
    {
        this.repository = repository;
        GetProductsAsync();
    }

    public async void GetProductsAsync()
    {
        var config = Configuration.Default.WithDefaultLoader();
        var document = BrowsingContext.New(config).OpenAsync(Reference).Result;
        var value_parsing = document.QuerySelectorAll(Selector);

        foreach (var value in value_parsing)
        {
            string name_bank = value.Children.Cast<IHtmlAnchorElement>().ToArray().FirstOrDefault().TextContent;
            BankName bank_name = new BankName(name_bank);

            bool contains_bank_name = await repository.IsContainsBankName(bank_name);

            if (!contains_bank_name)
            {
                await repository.AddBankName(bank_name);
            }

            var value_href = value.Children.Cast<IHtmlAnchorElement>();

            foreach (var elem in value_href)
            {
                  await Pars(bank_name, elem.Href);
            }
        }
    }
    private async Task Pars(BankName name, string link)
    {
        var config = Configuration.Default.WithDefaultLoader();
        var document = await BrowsingContext.New(config).OpenAsync(link);

        string name_contributed = document.QuerySelectorAll("h1.t-main-header.t-bold")
            .Select(value => value.InnerHtml)
            .FirstOrDefault();

        IEnumerable<string> value_title = document.QuerySelectorAll("span.wbl68j-4.oEvlI")
            .Select(value => value.InnerHtml.Replace("<!-- -->:", ""));

        IEnumerable<string> value_description = document.QuerySelectorAll("span.wbl68j-5.iDmuvl.t-bold")
            .Select(value => value.InnerHtml.Replace("<span>", "").Replace("</span>", ""));

        Dictionary<string, string> titles_descriptions = new Dictionary<string, string>();
        value_title.Zip(value_description).ToList().ForEach(tuple => titles_descriptions.Add(tuple.First, tuple.Second));

        decimal rate = default;
        string capitalization = default;
        string replenishment = default;
        string partial_removal = default;
        string payment_interest = default;
        decimal deposit_amount_from = default;
        decimal deposit_amount_to = default;
        long term_from = default;
        long term_to = default;

        foreach (var title_description in titles_descriptions)
        {
            if (title_description.Key == "Капитализация") capitalization = title_description.Value;
            if (title_description.Key == "Пополнение") replenishment = title_description.Value;
            if (title_description.Key == "Частичное снятие") partial_removal = title_description.Value;
            if (title_description.Key == "Выплата процентов") payment_interest = title_description.Value;
            if (title_description.Key == "Ставка") rate = decimal.Parse(title_description.Value.Replace("до", "").Replace("%", "").Trim());
            if (title_description.Key == "Сумма вклада")
            {
                var deposit_amount = GetDepositAmount(title_description.Value);
                if (deposit_amount is (null, null)) break;
                deposit_amount_from = (decimal)deposit_amount.deposit_from;
                deposit_amount_to = (decimal)deposit_amount.deposit_to;
            }
            if (title_description.Key == "Срок")
            {
                var term = GetTerm(title_description.Value);
                term_from = term.term_from;
                term_to = term.term_to;
            }
        }

        Contribution contribution = new Contribution(name_contributed, rate, deposit_amount_from, deposit_amount_to, term_from, term_to, capitalization, 
            payment_interest, replenishment, partial_removal, name.Name);

        bool contains_contribution = await repository.IsContainsContribution(contribution);

        if (!contains_contribution)
              await repository.AddContribution(contribution);
    }

    private (decimal? deposit_from, decimal? deposit_to) GetDepositAmount(string deposit)
    {
        decimal deposit_from = default;
        decimal deposit_to = default;

        if (deposit.Contains('$') || deposit.Contains('€') || deposit.Contains('£') || deposit.Contains('Ұ') || deposit.Contains('₣')) return (null, null);

        string[] split = deposit.Split('-');
        if (split.Length == 2)
        {
            deposit_from = decimal.Parse(split[0].Replace("₽", "").Replace(" ", "").Trim());
            deposit_to = decimal.Parse(split[1].Replace("₽", "").Replace(" ", "").Trim());
        }
        else if (split.Length == 1)
        {
            deposit_from = decimal.Parse(split[0].Replace("₽", "").Replace("от", "").Replace(" ", "").Trim());
            deposit_to = 50000000;
        }

        return (deposit_from, deposit_to);
    }
    private (long term_from, long term_to) GetTerm(string term)
    {
        long term_from = default;
        long term_to = default;

        string[] term_split = term.Split('-');
        if (term_split.Length == 2)
        {
            term_from = long.Parse(term_split[0].Replace("дней", "").Replace("день", "").Replace("дня", "").Trim());
            term_to = long.Parse(term_split[1].Replace("дней", "").Replace("день", "").Replace("дня", "").Trim());
        }
        else if (term_split.Length == 1)
        {
            if (term_split[0].Contains("год") || term_split[0].Contains("лет") || term_split[0].Contains("года"))
            {
                term_from = 31;
                term_to = long.Parse(term_split[0].Replace("года", "").Replace("год", "").Replace("лет", "").Replace(" ", "").Trim()) * (long)DaysOfYear;
            }
            else if (term_split[0].Contains("дней") || term_split[0].Contains("дня") || term_split[0].Contains("день"))
            {
                term_from = 31;
                term_to = long.Parse(term_split[0].Replace("дней", "").Replace("день", "").Replace("дня", "").Replace("от", "").Replace(" ", "").Trim()) * (long)DaysOfYear;
            }
        }

        return (term_from, term_to);
    }
}