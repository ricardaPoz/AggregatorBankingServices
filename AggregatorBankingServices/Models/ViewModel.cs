using AggregatorBankingServices.DataBase;
using AggregatorBankingServices.Helpers;
using AggregatorBankingServices.Interaction.Interfaces;
using AggregatorBankingServices.Interaction.Model;
using AggregatorBankingServices.Interaction.ResponseEF;
using AggregatorBankingServices.ParsingBankingServices;
using AggregatorBankingServices.ParsingBankingServices.Interfaces;
using AggregatorBankingServices.View;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace AggregatorBankingServices.Models;
public static class ViewModel
{
    public static ObservableCollection<Loan> Loans { get; private set; }
    public static ObservableCollection<BankName> BankNamesComboBox { get; private set; }
    public static ObservableCollection<TypePaymant> TypePaymentComboBox { get; private set; }
    public static ObservableCollection<Contribution> Contribution { get; private set; }
    public static ObservableCollection<Capitalization> Capitalization { get; private set; }

    public static User User { get; set; }

    public static event Action<bool, string> AuthorizationAccepted;
    public static event Action<bool, string> RegistrationAccepted;

    private static readonly IDataBaseResponse response;
    private static readonly IDataBaseResponseForParsing responsePS;

    private static ICommand registration_command;
    private static ICommand authorization_command;
    private static ICommand fill_name_bank_combo_box_command;
    private static ICommand fiil_type_payment_combobox;
    private static ICommand find_loan_products;
    private static ICommand get_all_loan;
    private static ICommand get_all_contribution;
    private static ICommand get_type_capitalization;
    private static ICommand find_contribution_products;

    static ViewModel()
    {
        response = new EFDataBaseRepository();
        responsePS = new EFDataBaseRepository();
        Loans = new ObservableCollection<Loan>();
        BankNamesComboBox = new ObservableCollection<BankName>();
        TypePaymentComboBox = new ObservableCollection<TypePaymant>();
        Contribution = new ObservableCollection<Contribution>();
        Capitalization = new ObservableCollection<Capitalization>();
    }


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

        User = new User(login, BCrypt.Net.BCrypt.HashPassword(password), null);

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
