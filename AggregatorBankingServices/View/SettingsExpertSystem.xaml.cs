using AggregatorBankingServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AggregatorBankingServices.ExpertSystem.Models;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;

namespace AggregatorBankingServices.View
{
    public partial class SettingsExpertSystem : Window
    {
        public SettingsExpertSystem()
        {
            InitializeComponent();
            ViewModel.GetAllDomains.Execute(null);
            ViewModel.GetAllDomainValues.Execute(null);
            ViewModel.GetAllVariableType.Execute(null);
            ViewModel.GetAllFacts.Execute(null);
            ViewModel.GetAllVariable.Execute(null);
            ViewModel.GetAllRules.Execute(null);
        }

        private void btn_add_domain_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void container_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) DragMove();
        }

        private void cmbUncoverAndHide_Unchecked(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Normal;
        }

        private void closeForm_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        private void cmbUncoverAndHide_Checked(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Maximized;
        }

        private void btn_add_fact_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_change_fact_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_remove_domain_fact_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_add_variable_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_change_variable_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_remove_variable_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_change_domain_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_remove_domain_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_add_domain_value_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_change_domain_value_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_remove_domain_value_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_add_rule_value_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_remove_rule_value_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_change_rule_value_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
