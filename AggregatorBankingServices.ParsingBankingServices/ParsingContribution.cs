using AggregatorBankingServices.Interaction.Model;
using AggregatorBankingServices.ParsingBankingServices.Interfaces;
using AngleSharp;
using AngleSharp.Html.Dom;

namespace AggregatorBankingServices.ParsingBankingServices;
public class ParsingContribution
{
    const string Reference = "https://www.sravni.ru/kredity/vse-produkty/";
    const double DaysOfYear = 365.5;
    const double DaysOfMonth = 30.417;
    const string Selector = "div.e7y1t0-2.bJEWIQ";

    private IDataBaseResponseForParsing repository;
    public ParsingContribution(IDataBaseResponseForParsing repository)
    {
        this.repository = repository;
        GetProducts();
    }

    public void GetProducts()
    {
        var config = Configuration.Default.WithDefaultLoader();
        var document = BrowsingContext.New(config).OpenAsync(Reference).Result;
        var value_parsing = document.QuerySelectorAll(Selector);

        value_parsing.ToList().ForEach(value =>
        {
            BankName bank_name = new BankName();
            bank_name.Name = value.Children.Cast<IHtmlAnchorElement>().ToArray()[0].TextContent;

            if (!repository.IsContainsBankName(bank_name))
            {
                repository.AddBankName(bank_name);
            }
        });

        foreach (var value in value_parsing)
        {
            BankName bank_name = new BankName();
            bank_name.Name = value.Children.Cast<IHtmlAnchorElement>().ToArray()[0].TextContent;

            value.Children.Cast<IHtmlAnchorElement>().ToList().ForEach(async elem =>
            {
                await Pars(bank_name, elem.Href);
            });
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

        Contribution contribution = new Contribution();
        contribution.NameBank = name.Name;
        contribution.Name = name_contributed;
        foreach (var title_description in titles_descriptions)
        {
            if (title_description.Key == "Капитализация") contribution.Capitalization = title_description.Value;
            if (title_description.Key == "Пополнение") contribution.Replenishment = title_description.Value;
            if (title_description.Key == "Частичное снятие") contribution.PartialRemoval = title_description.Value;
            if (title_description.Key == "Выплата процентов") contribution.PaymentInterest = title_description.Value;
            if (title_description.Key == "Ставка") contribution.Rate = decimal.Parse(title_description.Value.Replace("до", "").Replace("%", "").Trim());
            if (title_description.Key == "Сумма вклада")
            {
                var deposit_amount = GetDepositAmount(title_description.Value);
                if (deposit_amount is (null, null)) break;
                contribution.DepositAmountFrom = (decimal)deposit_amount.from;
                contribution.DepositAmountTo = (decimal)deposit_amount.to;
            }
            if (title_description.Key == "Срок")
            {
                var term = GetTerm(title_description.Value);
                contribution.TermFrom = term.from;
                contribution.TermTo = term.to;
            }
        }
        if (!repository.IsContainsContribution(contribution))
            repository.AddContribution(contribution);




    }
}
