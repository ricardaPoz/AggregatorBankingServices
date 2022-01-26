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
using System.Windows.Navigation;
using System.Windows.Shapes;
using AggregatorBankingServices.DataBase;
using AggregatorBankingServices.Interaction.Interfaces;
using AggregatorBankingServices.Interaction.Model;
using AggregatorBankingServices.Models;
using AggregatorBankingServices.ParsingBankingServices;
using AggregatorBankingServices.ParsingBankingServices.Interfaces;

namespace AggregatorBankingServices
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ViewModel.GetAllContribution.Execute(null);
            //.FillTypePayment.Execute(null);
        }

        private void button_clik_find_loan_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.FindLoanProducts
                .Execute(new Tuple<object, object, object, object, object>(ComboBoxNameBank.SelectedValue, ComboBoxTypePayment.SelectedItem, RateSlider.Value, SliderSumFrom.Value, SliderSumTo.Value));
        }

        private void button_clear_Click(object sender, RoutedEventArgs e)
        {
            ComboBoxNameBank.SelectedItem = null;
            ComboBoxTypePayment.SelectedItem = null;
            RateSlider.Value = 3;
            SliderSumFrom.Value = 1;
            SliderSumTo.Value = 1;
        }
    }
}
