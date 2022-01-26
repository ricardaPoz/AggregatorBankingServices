using AggregatorBankingServices.DataBase;
using AggregatorBankingServices.Models;
using AggregatorBankingServices.ParsingBankingServices;
using AggregatorBankingServices.ParsingBankingServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AggregatorBankingServices.View
{
    /// <summary>
    /// Логика взаимодействия для BankingProducts.xaml
    /// </summary>
    public partial class BankingProducts : Window
    {
        public BankingProducts()
        {
            InitializeComponent();
            /*ViewModel.FillBankNameComboBoxCommand.Execute(null);
            ViewModel.FillTypePayment.Execute(null);*/

            IDataBaseResponseForParsing cal = new EFDataBaseRepository();
            ParsingContribution parsing = new ParsingContribution(cal);
        }

        private void container_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) DragMove();
        }

        private void closeForm_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        private void cmbUncoverAndHide_Checked(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Maximized;
        }

        private void cmbUncoverAndHide_Unchecked(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Normal;
        }

        private void button_clear_Click(object sender, RoutedEventArgs e)
        {
            ComboBoxNameBank.SelectedItem = null;
            ComboBoxTypePayment.SelectedItem = null;
            RateSlider.Value = 3;
            SliderSumFrom.Value = 1;
            SliderSumTo.Value = 1;
        }

        private void button_clik_find_loan_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.FindLoanProducts
                .Execute(new Tuple<object, object, object, object, object>(ComboBoxNameBank.SelectedValue, ComboBoxTypePayment.SelectedItem, RateSlider.Value, SliderSumFrom.Value, SliderSumTo.Value));
        }

        private void button_all_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.GetAllLoan.Execute(null);
        }
    }
}
