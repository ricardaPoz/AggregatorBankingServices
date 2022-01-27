using AggregatorBankingServices.DataBase.Models;
using AggregatorBankingServices.ParsingBankingServices.Interfaces;

using BankName = AggregatorBankingServices.Interaction.Model.BankName;
using BankNameData = AggregatorBankingServices.DataBase.Models.BankName;
using Contribution = AggregatorBankingServices.Interaction.Model.Contribution;
using ContributionData = AggregatorBankingServices.DataBase.Models.Contribution;
using Loan = AggregatorBankingServices.Interaction.Model.Loan;
using LoanData = AggregatorBankingServices.DataBase.Models.Loan;
using User = AggregatorBankingServices.Interaction.Model.User;
using UserData = AggregatorBankingServices.DataBase.Models.User;

using AutoMapper;
using AggregatorBankingServices.Interaction.Interfaces;
using AggregatorBankingServices.Interaction.ResponseEF;

namespace AggregatorBankingServices.DataBase;
public class EFDataBaseRepository : IDataBaseResponseForParsing, IDataBaseResponse
{
    private readonly BankProductDataBaseContext db_context = new BankProductDataBaseContext();
    private readonly IMapper mapper;
    private readonly IMapper mapper_for_view;
    private readonly object locker = new object();
    public EFDataBaseRepository()
    {
        var config = new MapperConfiguration(conf =>
        {
            conf.CreateMap<BankName, BankNameData>();
            conf.CreateMap<Contribution, ContributionData>()
                .ForMember(contribution_data => contribution_data.NameBank,
                 conf => conf.MapFrom(contribution_parsing => contribution_parsing.NameBank));
            conf.CreateMap<Loan, LoanData>();
            conf.CreateMap<User, UserData>();
        });
        mapper = config.CreateMapper();


        var config_view = new MapperConfiguration(conf =>
        {
            conf.CreateMap<LoanData, Loan>();
            conf.CreateMap<BankNameData, BankName>();
            conf.CreateMap<ContributionData, Contribution>();
        });
        mapper_for_view = config_view.CreateMapper();
    }
    public async Task AddBankName(BankName bank_name)
    {
        lock (locker)
        {
            BankNameData bank_name_data = mapper.Map<BankNameData>(bank_name);
            db_context.BankNames.Add(bank_name_data);
            db_context.SaveChanges();
        }
    }
    public async Task AddContribution(Contribution contribution)
    {
        lock (locker)
        {
            if (contribution.Capitalization is not null && contribution.Replenishment is not null)
            {
                ContributionData contribution_data = mapper.Map<ContributionData>(contribution);
                db_context.Contributions.Add(contribution_data);
                db_context.SaveChanges();
            }
        }
    }

    public async Task AddLoan(Loan loan)
    {
        lock (locker)
        {
            LoanData loan_data = mapper.Map<LoanData>(loan);
            db_context.Loans.Add(loan_data);
            db_context.SaveChanges();
        }
    }

    public async Task<(bool is_contains, string password)> GetUserNameAndPassword(User user)
    {
        lock (locker)
        {
            bool is_completed = false;
            UserData user_data = mapper.Map<UserData>(user);
            bool contains_user = db_context.Users.Any(user => user.Login == user_data.Login);
            if (contains_user)
            {
                var password = db_context.Users.Where(user => user.Login == user_data.Login).Select(pass => pass.Password).FirstOrDefault();
                var scoring = db_context.Users.Where(user => user.Login == user_data.Login).Select(scoring => scoring.Scoring).FirstOrDefault();

                return (is_completed = true, password);
            }
            else return (is_completed, null);
        }
    }

    public async Task<(bool is_contains, string scoring)> GetScoringBall(string login)
    {
        lock (locker)
        {
            bool is_completed = false;
            bool contains_user = db_context.Users.Any(user => user.Login == login);
            if (contains_user)
            {
                var scoring = db_context.Users.Where(user => user.Login == login).Select(scoring => scoring.Scoring).FirstOrDefault();

                return (is_completed = true, scoring);
            }
            else return (is_completed, null);
        }
    }

    public async Task<bool> IsContainsBankName(BankName bank_name)
    {
        lock (locker)
        {
            BankNameData bank_name_data = mapper.Map<BankNameData>(bank_name);
            return db_context.BankNames.Any(bank => bank.Name == bank_name.Name);
        }
    }
    public async Task<bool> IsContainsContribution(Contribution contribution)
    {
        lock (locker)
        {
            ContributionData contribution_data = mapper.Map<ContributionData>(contribution);
            return db_context.Contributions.Any(contribution => contribution.Name == contribution_data.Name);
        }
    }

    public async Task<bool> IsContainsLoan(Loan loan)
    {
        lock (locker)
        {
            LoanData loan_data = mapper.Map<LoanData>(loan);
            return db_context.Loans.Any(loan => loan.Name == loan_data.Name && loan.Rate == loan_data.Rate);
        }
    }

    public async Task<bool> UserRegistration(User user)
    {
        lock (locker)
        {
            bool is_completed = false;
            UserData user_data = mapper.Map<UserData>(user);
            bool contains_user = db_context.Users.Any(user => user.Login == user_data.Login);
            if (!contains_user)
            {
                db_context.Users.Add(user_data);
                db_context.SaveChanges();
                return is_completed = true;
            }
            else return is_completed;
        }
    }

    public async Task<IEnumerable<Loan>> GetAllLoan()
    {
        lock (locker)
        {
            IEnumerable<LoanData> loans = db_context.Loans;
            return mapper_for_view.ProjectTo<Loan>(loans.AsQueryable());
        }
    }
    public async Task<IEnumerable<BankName>> GetAllBankNames()
    {
        lock (locker)
        {
            IEnumerable<BankNameData> bank_names = db_context.BankNames;
            return mapper_for_view.ProjectTo<BankName>(bank_names.AsQueryable());
        }
    }

    public async Task<IEnumerable<TypePaymant>> GetTypePayment()
    {
        lock (locker)
        {
            IEnumerable<TypePaymant> unique_loan_type_payment = db_context.Loans
           .Select(loan => loan.TypePayment)
           .Distinct()
           .Where(value => value != null)
           .Select(type_payment => new TypePaymant(type_payment));
            return unique_loan_type_payment;
        }

    }

    public async Task<IEnumerable<Loan>> SearchLoans(BankName bank_name, TypePaymant type_payment, double rate, double loan_amount_from, double loan_amount_to)
    {
        lock (locker)
        {
            IEnumerable<LoanData> loans = db_context.Loans
            .Where(loan => loan.Rate == (decimal)rate && loan.LoanAmountFrom >= (decimal)loan_amount_from && loan.LoanAmountTo <= (decimal)loan_amount_to);
            if (bank_name is not null)
            {
                loans = loans
                    .Where(loan => loan.NameBank == bank_name.Name);
            }
            if (type_payment is not null)
            {
                loans = loans
                    .Where(loan => loan.TypePayment == type_payment.TypePaymantValue);
            }

            return mapper_for_view.ProjectTo<Loan>(loans.AsQueryable());
        }
    }

    public async Task<IEnumerable<Contribution>> GetAllContribution()
    {
        lock (locker)
        {
            IEnumerable<ContributionData> contributions = db_context.Contributions;
            return mapper_for_view.ProjectTo<Contribution>(contributions.AsQueryable());
        }

    }

    public async Task<IEnumerable<Capitalization>> GetTypeCapitalization()
    {
        lock (locker)
        {
            IEnumerable<Capitalization> unique_type_capitalization = db_context.Contributions
           .Select(loan => loan.Capitalization)
           .Distinct()
           .Where(value => value != null)
           .Select(type_capitalization => new Capitalization(type_capitalization));
            return unique_type_capitalization;
        }
    }

    public async Task<IEnumerable<Contribution>> SearchContribution(BankName bank_name, Capitalization type_capitalization, double rate, double loan_amount_from, double loan_amount_to)
    {
        lock (locker)
        {
            IEnumerable<ContributionData> loans = db_context.Contributions
            .Where(contribution => contribution.Rate == (decimal)rate && contribution.DepositAmountFrom >= (decimal)loan_amount_from && contribution.DepositAmountTo <= (decimal)loan_amount_to);
            if (bank_name is not null)
            {
                loans = loans
                    .Where(loan => loan.NameBank == bank_name.Name);
            }
            if (type_capitalization is not null)
            {
                loans = loans
                    .Where(loan => loan.Capitalization == type_capitalization.Value);
            }

            return mapper_for_view.ProjectTo<Contribution>(loans.AsQueryable());
        }
    }

    public async Task SetScoring(string login, string scoring)
    {
        lock (locker)
        {
            var user = db_context.Users.Where(user => user.Login == login).FirstOrDefault();
            user.Scoring = scoring;
            db_context.SaveChanges();
        }
    }
}
