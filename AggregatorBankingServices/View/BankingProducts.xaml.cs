﻿using AggregatorBankingServices.DataBase;
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
            ViewModel.FillBankNameComboBoxCommand.Execute(null);
            ViewModel.FillTypePayment.Execute(null);
            ViewModel.GetTypeCapitalization.Execute(null);
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

        private void button_clear_contributions_Click(object sender, RoutedEventArgs e)
        {
            ComboBoxNameBankContributions.SelectedItem = null;
            ComboBoxCapitalization.SelectedItem = null;
            RateSliderContributed.Value = 3;
            SliderSumFromContributions.Value = 1;
            SliderSumToContributions.Value = 1;
        }

        private void button_clik_find_contributions_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.FindContributionProducts
                .Execute(new Tuple<object, object, object, object, object>(ComboBoxNameBankContributions.SelectedValue, ComboBoxCapitalization.SelectedItem, RateSliderContributed.Value, SliderSumFromContributions.Value, SliderSumToContributions.Value));
        }

        private void button_all_contribution_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.GetAllContribution.Execute(null);
        }

        private void button_testing_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.TestingUser.Execute(null);
        }
    }
}
