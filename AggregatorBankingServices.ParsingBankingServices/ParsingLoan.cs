using AggregatorBankingServices.Interaction.Model;
using AggregatorBankingServices.ParsingBankingServices.Interfaces;
using AngleSharp;
using AngleSharp.Html.Dom;
public class ParsingLoan
{
    const string Reference = "https://www.sravni.ru/kredity/vse-produkty/";
    const double DaysOfYear = 365.5;
    const double DaysOfMonth = 30.417;
    const string Selector = "div.e7y1t0-2.bJEWIQ";

    private IDataBaseResponseForParsing repository;
    public ParsingLoan(IDataBaseResponseForParsing repository)
    {
        this.repository = repository;
        GetProducts();
    }
    private async void GetProducts()
    {
        var config = Configuration.Default.WithDefaultLoader();
        var document = await BrowsingContext.New(config).OpenAsync(Reference);
        var value_parsing = document.QuerySelectorAll(Selector).Skip(1);

        foreach (var value in value_parsing)
        {
            BankName bank_name = new BankName();
            bank_name.Name = value.Children.Cast<IHtmlAnchorElement>().FirstOrDefault().TextContent;

            bool contains_bank_name = await repository.IsContainsBankName(bank_name);

            if (!contains_bank_name)
            {
                await repository.AddBankName(bank_name);
            }

            var value_href = value.Children.Cast<IHtmlAnchorElement>().Skip(1);

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

        string name_loan = document.QuerySelectorAll("h1.o2gw6t-0.eeztOV.qa-headerPage")
            .Select(value => value.InnerHtml)
            .FirstOrDefault();

        IEnumerable<string> loan_title = document.QuerySelectorAll("tr.svkpc1-1.byELtA")
            .Select(element => element.FirstChild.TextContent);

        IEnumerable<string> loan_description = document.QuerySelectorAll("tr.svkpc1-1.byELtA")
            .Select(element => element.LastChild.TextContent);

        var rate = document.QuerySelectorAll("div.sc-1n6ngty-0.eNrZuG")
            .Select(element =>
            {
                return (element.FirstChild.TextContent, element.LastChild.TextContent);
            }).FirstOrDefault();

        Dictionary<string, string> titles_descriptions = new Dictionary<string, string>();
        var value = loan_title.Zip(loan_description);

        foreach (var item in value)
        {
            if (!titles_descriptions.ContainsKey(item.First))
                titles_descriptions.Add(item.First, item.Second);
        }

        if (rate.Item1 is not null && rate.Item2 is not null)
        {
            titles_descriptions.Add(rate.Item1, rate.Item2);

            decimal loan_rate = default;
            decimal loan_amount_from = default;
            decimal loan_amount_to = default;
            string type_payment = default;
            string payment_frequency = default;
            string application_review = default;
            string mandatory_insurance = default;
            string required_documents = default;
            string income_verification = default;
            long age_redemption_man = default;
            long age_redemption_women = default;
            long borrower_age = default;
            long term_from = default;
            long term_to = default;

            foreach (var title_description in titles_descriptions)
            {
                if (title_description.Key.Trim() == "Ставка") loan_rate = GetInterestCredit(title_description.Value);
                if (title_description.Key.Trim() == "Сумма кредита")
                {
                    if (title_description.Value == "Любая") break;
                    var loan_amount = GetLoanAmmount(title_description.Value);
                    loan_amount_from = loan_amount.amount_from;
                    loan_amount_to = loan_amount.amount_to;
                }
                if (title_description.Key.Trim() == "Тип платежей") type_payment = title_description.Value;
                if (title_description.Key.Trim() == "Периодичность платежей") payment_frequency = title_description.Value;
                if (title_description.Key.Trim() == "Рассмотрение заявки") application_review = title_description.Value;
                if (title_description.Key.Trim() == "Обязательная страховка") mandatory_insurance = title_description.Value;
                if (title_description.Key.Trim() == "Обязательные документы") required_documents = title_description.Value;
                income_verification = default;
                if (title_description.Key.Trim() == "Подтверждение дохода"
                    || title_description.Key.Trim() == "Подтверждение дохода (1 документ на выбор)")
                    income_verification = title_description.Value;
                if (title_description.Key.Trim() == "Возраст при погашении")
                {
                    var age_redemptation = AgeRedemption(title_description.Value);
                    age_redemption_man = age_redemptation.age_redemption_man;
                    age_redemption_women = age_redemptation.age_redemption_women;
                }
                if (title_description.Key.Trim() == "Возраст заемщика") borrower_age = GetBorrowerAge(title_description.Value);

                if (title_description.Key.Trim() == "Срок")
                {
                    var term = GetTerm(title_description.Value);
                    term_from = term.term_from;
                    term_to = term.term_to;
                }
            }
            Loan loan = new Loan(name_loan, loan_rate, loan_amount_from, loan_amount_to, type_payment, payment_frequency, application_review, mandatory_insurance,
                required_documents, income_verification, age_redemption_man, age_redemption_women, borrower_age, term_from, term_to, name.Name);
            bool contains_bank_name = await repository.IsContainsLoan(loan);
            if (!contains_bank_name)
                await repository.AddLoan(loan);
        }
    }
    private decimal GetInterestCredit(string interest)
    {
        interest = interest.Replace("от", "").Replace("%", "").Trim();
        decimal result = decimal.Parse(interest);
        return result;
    }
    private (decimal amount_from, decimal amount_to) GetLoanAmmount(string loan_amount)
    {
        string loan_amount_from = default;
        string loan_amount_to = default;
        loan_amount = loan_amount.Replace("₽", "").Replace("–", "/");
        var split_loan_amount = loan_amount.Split('/');
        if (split_loan_amount.Length > 1)
        {
            loan_amount_from = split_loan_amount[0];
            loan_amount_to = split_loan_amount[1];
        }
        else if (split_loan_amount.Length == 1)
        {
            loan_amount_from = split_loan_amount[0];
            loan_amount_to = split_loan_amount[0];
        }

        decimal result_loan_amount_from = decimal.Parse(loan_amount_from);
        decimal result_loan_amount_to = decimal.Parse(loan_amount_to);

        return (result_loan_amount_from, result_loan_amount_to);
    }
    private (long age_redemption_man, long age_redemption_women) AgeRedemption(string age_redemption)
    {
        string age_redemption_man = default;
        string age_redemption_women = default;
        age_redemption = age_redemption.Replace("<!-- -->", "").Replace("(муж.)", "").Replace("(жен.)", "").Trim();
        var split_age = age_redemption.Split("/");
        age_redemption_man = split_age[0].Replace("до", "").Replace("года", "").Replace("лет", "").Trim();
        if (split_age.Length == 1) age_redemption_women = split_age[0].Replace("до", "").Replace("года", "").Replace("лет", "").Trim();
        else age_redemption_women = split_age[1].Replace("до", "").Replace("года", "").Replace("лет", "").Trim();

        return (TimeSpan.Parse($"{Math.Round(double.Parse(age_redemption_man) * DaysOfYear)}").Days, TimeSpan.Parse($"{Math.Round(double.Parse(age_redemption_women) * DaysOfYear)}").Days);
    }
    private long GetBorrowerAge(string borrower_age)
    {
        borrower_age = borrower_age.Replace("от", "").Replace("лет", "").Replace("года", "").Trim();
        return TimeSpan.Parse($"{Math.Round(double.Parse(borrower_age) * DaysOfYear)}").Days;
    }
    private (long term_from, long term_to) GetTerm(string term_string)
    {
        string term_from_string = default;
        string term_to_string = default;
        long term_from = default;
        long term_to = default;

        term_string = term_string.Replace("–", "/");
        string[] temp_term = term_string.Split("/");

        switch (temp_term.Length)
        {
            case 1:
                if (temp_term[0].Contains("год") || temp_term[0].Contains("года"))
                {
                    term_to_string = temp_term[0].Replace("года", "").Replace("год", "").Trim();
                    term_to = TimeSpan.Parse($"{Math.Round(double.Parse(term_to_string) * DaysOfYear)}").Days;
                    term_from = TimeSpan.Parse($"{Math.Round(double.Parse(term_to_string) * DaysOfYear)}").Days;
                }
                else if (temp_term[0].Contains("месяцев") || temp_term[0].Contains("месяца"))
                {
                    term_to_string = temp_term[0].Replace("месяцев", "").Replace("месяца", "").Trim();
                    term_to = TimeSpan.Parse($"{Math.Round(double.Parse(term_to_string) * DaysOfMonth)}").Days;
                    term_from = TimeSpan.Parse($"{Math.Round(double.Parse(term_to_string) * DaysOfMonth)}").Days;
                }
                break;
            case 2:
                if (temp_term[0].Contains("дней"))
                {
                    term_from_string = temp_term[0].Replace("дней", "").Trim();
                    term_from = TimeSpan.Parse(term_from_string).Days;
                }
                else if (temp_term[0].Contains("месяцев") || temp_term[0].Contains("месяц") || temp_term[0].Contains("месяца"))
                {
                    term_from_string = temp_term[0].Replace("месяцев", "").Replace("месяца", "").Replace("месяц", "").Trim();
                    term_from = TimeSpan.Parse($"{Math.Round(double.Parse(term_from_string) * DaysOfMonth)}").Days;
                }
                else
                {
                    if (temp_term[1].Contains("месяцев") || temp_term[1].Contains("месяца"))
                    {
                        term_from_string = temp_term[0].Trim();
                        term_to_string = temp_term[1].Replace("месяцев", "").Replace("месяца", "").Trim();
                        term_from = TimeSpan.Parse($"{Math.Round(double.Parse(term_from_string) * DaysOfMonth)}").Days;
                        term_to = TimeSpan.Parse($"{Math.Round(double.Parse(term_to_string) * DaysOfMonth)}").Days;
                    }
                    else if (temp_term[1].Contains("лет") || temp_term[1].Contains("года"))
                    {
                        term_from_string = temp_term[0].Trim();
                        term_to_string = temp_term[1].Replace("года", "").Replace("лет", "").Trim();
                        term_from = TimeSpan.Parse($"{Math.Round(double.Parse(term_from_string) * DaysOfYear)}").Days;
                        term_to = TimeSpan.Parse($"{Math.Round(double.Parse(term_to_string) * DaysOfYear)}").Days;
                    }
                    else if (temp_term[1].Contains("дней"))
                    {
                        term_from_string = temp_term[0].Trim();
                        term_to_string = temp_term[1].Replace("дней", "").Trim();
                        term_from = TimeSpan.Parse(term_from_string).Days;
                        term_to = TimeSpan.Parse(term_to_string).Days;
                    }

                }
                if (temp_term[1].Contains("месяцев") || temp_term[1].Contains("месяца"))
                {
                    term_to_string = temp_term[1].Replace("месяцев", "").Replace("месяца", "").Trim();
                    term_to = TimeSpan.Parse($"{Math.Round(double.Parse(term_to_string) * DaysOfMonth)}").Days;
                }
                else if (temp_term[1].Contains("лет") || temp_term[1].Contains("года"))
                {
                    term_to_string = temp_term[1].Replace("года", "").Replace("лет", "").Trim();
                    term_to = TimeSpan.Parse($"{Math.Round(double.Parse(term_to_string) * DaysOfYear)}").Days;
                }
                break;
        }
        return (term_from, term_to);
    }
}
