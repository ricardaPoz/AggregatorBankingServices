using AggregatorBankingServices.DataBase;
using AggregatorBankingServices.ExpertSystem.ExplanatoryComponent.Interfaces;
using AggregatorBankingServices.ExpertSystem.Interfaces;
using AggregatorBankingServices.ExpertSystem.Models;
using AggregatorBankingServices.ExpertSystem.Repository;
using AggregatorBankingServices.Helpers;
using AggregatorBankingServices.Interaction.Interfaces;
using AggregatorBankingServices.Interaction.Model;
using AggregatorBankingServices.Interaction.ResponseEF;
using AggregatorBankingServices.KnowledgeBase;
using AggregatorBankingServices.ParsingBankingServices.Interfaces;
using AggregatorBankingServices.View;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace AggregatorBankingServices.Models;
public static class ViewModel
{
    public static ObservableCollection<Loan> Loans { get; private set; }
    public static ObservableCollection<BankName> BankNamesComboBox { get; private set; }
    public static ObservableCollection<TypePaymant> TypePaymentComboBox { get; private set; }
    public static ObservableCollection<Contribution> Contribution { get; private set; }
    public static ObservableCollection<Capitalization> Capitalization { get; private set; }

    public static ObservableCollection<Domain> Domains { get; private set; }
    public static ObservableCollection<DomainValue> DomainValue { get; private set; }
    public static ObservableCollection<VariableType> VariableTypes { get; private set; }
    public static ObservableCollection<Fact> Facts { get; private set; }
    public static ObservableCollection<Variable> Variables { get; private set; }
    public static ObservableCollection<Rule> Rules { get; private set; }

    public static User User { get; set; }

    public static event Action<bool, string> AuthorizationAccepted;
    public static event Action<bool, string> RegistrationAccepted;

    private static readonly IDataBaseResponse response;
    private static readonly IDataBaseResponseForParsing responsePS;
    private static readonly IExperSystemCRUT response_es;

    private static ICommand registration_command;
    private static ICommand authorization_command;
    private static ICommand fill_name_bank_combo_box_command;
    private static ICommand fiil_type_payment_combobox;
    private static ICommand find_loan_products;
    private static ICommand get_all_loan;
    private static ICommand get_all_contribution;
    private static ICommand get_type_capitalization;
    private static ICommand find_contribution_products;

    private static ICommand testing_user;
    private static ICommand change_scoring;

    private static ICommand get_all_domains;
    private static ICommand get_all_domain_values;
    private static ICommand get_all_variable_type;
    private static ICommand get_all_facts;
    private static ICommand get_all_variable;
    private static ICommand get_all_rules;


    static ViewModel()
    {
        response = new EFDataBaseRepository();
        responsePS = new EFDataBaseRepository();
        response_es = new EFKnowledgeBase();
        Loans = new ObservableCollection<Loan>();
        BankNamesComboBox = new ObservableCollection<BankName>();
        TypePaymentComboBox = new ObservableCollection<TypePaymant>();
        Contribution = new ObservableCollection<Contribution>();
        Capitalization = new ObservableCollection<Capitalization>();
        Domains = new ObservableCollection<Domain>();
        DomainValue = new ObservableCollection<DomainValue>();
        VariableTypes = new ObservableCollection<VariableType>();
        Facts = new ObservableCollection<Fact>();
        Variables = new ObservableCollection<Variable>();
        Rules = new ObservableCollection<Rule>();
    }
    public static ICommand GetAllRules => get_all_rules ?? (get_all_rules = new RelayCommand(async obj =>
    {
        var rules = await response_es.SelectRulesAsync();
        App.Current.Dispatcher.Invoke(() =>
        {
            Rules.Clear();
            foreach (var rule in rules)
            {
                Rules.Add(rule);
            }
        });
    }));
    public static ICommand GetAllVariable => get_all_variable ?? (get_all_variable = new RelayCommand(async obj =>
    {
        var variables = await response_es.SelectVariablesAsync();
        App.Current.Dispatcher.Invoke(() =>
        {
            Variables.Clear();
            foreach (var fact in variables)
            {
                Variables.Add(fact);
            }
        });
    }));
    public static ICommand GetAllFacts => get_all_facts ?? (get_all_facts = new RelayCommand(async obj =>
    {
        var facts = await response_es.SelectFactsAsync();
        App.Current.Dispatcher.Invoke(() =>
        {
            Facts.Clear();
            foreach (var fact in facts)
            {
                Facts.Add(fact);
            }
        });
    }));
    public static ICommand GetAllVariableType => get_all_variable_type ?? (get_all_variable_type = new RelayCommand(async obj =>
    {
        var variables_type = await response_es.SelectVaribleTypesAsync();
        App.Current.Dispatcher.Invoke(() =>
        {
            VariableTypes.Clear();
            foreach (var variable_type in variables_type)
            {
                VariableTypes.Add(variable_type);
            }
        });
    }));
    public static ICommand GetAllDomains => get_all_domains ?? (get_all_domains = new RelayCommand(async obj =>
    {
        var domains = await response_es.SelectDomainAsync();
        App.Current.Dispatcher.Invoke(() =>
        {
            Domains.Clear();
            foreach (var domain in domains)
            {
                Domains.Add(domain);
            }
        });
    }));
    public static ICommand GetAllDomainValues => get_all_domain_values ?? (get_all_domain_values = new RelayCommand(async obj =>
    {
        var domain_values = await response_es.SelectDomainValuesAsync();
        App.Current.Dispatcher.Invoke(() =>
        {
            DomainValue.Clear();
            foreach (var domain_value in domain_values)
            {
                DomainValue.Add(domain_value);
            }
        });
    }));
    public static ICommand ChangeScoring => change_scoring ?? (change_scoring = new RelayCommand(async obj =>
    {
        var (objOne, objTwo) = obj as Tuple<object, object>;
        if (!(objOne is string login) || !(objTwo is string scoring)) return;
        await response.SetScoring(login, scoring);
    }));
    public static ICommand TestingUser => testing_user ?? (testing_user = new RelayCommand(async obj =>
    {
        Testing testing = new Testing();
        testing.ShowDialog();
    }));
   
    public static ICommand FindContributionProducts => find_contribution_products ?? (find_contribution_products = new RelayCommand(async obj =>
    {
        var (obj1, obj2, obj3, obj4, obj5) = obj as Tuple<object, object, object, object, object>;
        BankName bank_name = (BankName)obj1;
        Capitalization type_capitalization = (Capitalization)obj2;
        double rate = (double)obj3;
        double sum_from = (double)obj4;
        double sum_to = (double)obj5;

        var contributions = await response.SearchContribution(bank_name, type_capitalization, rate, sum_from, sum_to);

        App.Current.Dispatcher.Invoke(() =>
        {
            Contribution.Clear();
            foreach (var contributions in contributions)
            {
                Contribution.Add(contributions);
            }
        });
    }));

    public static ICommand GetTypeCapitalization => get_type_capitalization ?? (get_type_capitalization = new RelayCommand(async obj =>
    {
        var all_capitalization = await response.GetTypeCapitalization();
        App.Current.Dispatcher.Invoke(() =>
        {
            foreach (var capitalization in all_capitalization)
            {
                Capitalization.Add(capitalization);
            }
        });
    }));
    public static ICommand GetAllContribution => get_all_contribution ?? (get_all_contribution = new RelayCommand(async obj =>
    {
        var all_contribution = await response.GetAllContribution();
        App.Current.Dispatcher.Invoke(() =>
        {
            Contribution.Clear();
            foreach (var contribution in all_contribution)
            {
                Contribution.Add(contribution);
            }
        });
    }));
    public static ICommand GetAllLoan => get_all_loan ?? (get_all_loan = new RelayCommand(async obj =>
    {
        var all_loan = await response.GetAllLoan();
        App.Current.Dispatcher.Invoke(() =>
        {
            Loans.Clear();
            foreach (var loan in all_loan)
            {
                Loans.Add(loan);
            }
        });
    }));
    public static ICommand FindLoanProducts => find_loan_products ?? (find_loan_products = new RelayCommand(async obj =>
    {
        var (obj1, obj2, obj3, obj4, obj5) = obj as Tuple<object, object, object, object, object>;
        BankName bank_name = (BankName)obj1;
        TypePaymant type_paymant = (TypePaymant)obj2;
        double rate = (double)obj3;
        double sum_from = (double)obj4;
        double sum_to = (double)obj5;

        var loans = await response.SearchLoans(bank_name, type_paymant, rate, sum_from, sum_to);

        App.Current.Dispatcher.Invoke(() =>
        {
            Loans.Clear();
            foreach (var loan in loans)
            {
                Loans.Add(loan);
            }
        });
    }));

    public static ICommand FillTypePayment => fiil_type_payment_combobox ?? (fiil_type_payment_combobox = new RelayCommand(async obj =>
    {
        var type_payment = await response.GetTypePayment();
        App.Current.Dispatcher.Invoke(() =>
        {
            foreach (var type_payment in type_payment)
            {
                TypePaymentComboBox.Add(type_payment);
            }
        });
    }));
    public static ICommand FillBankNameComboBoxCommand => fill_name_bank_combo_box_command ??= new RelayCommand(async obj =>
    {
        var bank_name = await response.GetAllBankNames();
        App.Current.Dispatcher.Invoke(() =>
        {
            foreach (var name in bank_name)
            {
                BankNamesComboBox.Add(name);
            }
        });
    });

    public static ICommand RegistrationCommand => registration_command ??= new RelayCommand(async obj =>
    {
        var (objOne, objTwo) = obj as Tuple<object, object>;
        if (!(objOne is string login) || !(objTwo is string password)) return;

        User user = new User(login, BCrypt.Net.BCrypt.HashPassword(password), null);

        bool is_contains = await response.UserRegistration(user);
        if (is_contains)
        {
            RegistrationAccepted?.Invoke(true, "Пользователь успешно зарегистрирован");
        }
        else RegistrationAccepted?.Invoke(false, "Такой пользователь уже существует");
    }
    );

    public static ICommand AuthorizationCommand => authorization_command ??= new RelayCommand(async obj =>
    {
        var (objOne, objTwo) = obj as Tuple<object, object>;
        if (!(objOne is string login) || !(objTwo is string password)) return;

        var scoring = await response.GetScoringBall(login);
        if (scoring.is_contains)
        {
            User = new User(login, BCrypt.Net.BCrypt.HashPassword(password), scoring.scoring);
        }
        else User = new User(login, BCrypt.Net.BCrypt.HashPassword(password), null);

        var user_data = await response.GetUserNameAndPassword(User);
        if (!user_data.is_contains)
        {
            AuthorizationAccepted?.Invoke(false, "Пользователь не найден");
            return;
        }
        else
        {
            if (BCrypt.Net.BCrypt.Verify(password, user_data.password))
            {
                BankingProducts window_banking_product = new BankingProducts();
                
                window_banking_product.ShowDialog();
            }
            else
            {
                AuthorizationAccepted?.Invoke(false, "Неверный логин или пароль");
            }
        }
    }
    );
}
