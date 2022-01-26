using AggregatorBankingServices.Models;
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
    /// Логика взаимодействия для Authorization.xaml
    /// </summary>
    public partial class Authorization : Window
    {
        public Authorization()
        {
            InitializeComponent();
            ViewModel.AuthorizationAccepted += AuthorizationAccepted;
            ViewModel.RegistrationAccepted += RegistrationAccepted;
        }

        private void RegistrationAccepted(bool registrationAccepted, string message)
        {
            if (registrationAccepted) DisplayNotification(Brushes.Green, message);
            else DisplayNotification(Brushes.Red, message);
        }

        private void AuthorizationAccepted(bool authorizationAccepted, string message)
        {
            if (authorizationAccepted) DisplayNotification(Brushes.Green, message);
            else DisplayNotification(Brushes.Red, message);
        }

        private void container_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) DragMove();
        }

        private void tbLogin_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            errorWrite.Visibility = Visibility.Collapsed;
            borderLogin.BorderBrush = new SolidColorBrush(Color.FromRgb(216, 216, 216));
            borderPassword.BorderBrush = new SolidColorBrush(Color.FromRgb(216, 216, 216));
        }

        private void Viewbox_MouseDown(object sender, MouseButtonEventArgs e) => Close();

        private void pbPassword_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            errorWrite.Visibility = Visibility.Collapsed;
            borderLogin.BorderBrush = new SolidColorBrush(Color.FromRgb(216, 216, 216));
            borderPassword.BorderBrush = new SolidColorBrush(Color.FromRgb(216, 216, 216));
        }

        private void pbPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            waterMark.Visibility = pbPassword.Password.Length > 0 ? waterMark.Visibility = Visibility.Collapsed : waterMark.Visibility = Visibility.Visible;
        }

        private void btnRegistration_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(pbPassword.Password) || string.IsNullOrEmpty(tbLogin.Text))
            {
                DisplayNotification(Brushes.Red, "Введите логин и пароль");
                borderPassword.BorderBrush = Brushes.Red;
                return;
            }
            else ViewModel.RegistrationCommand.Execute(new Tuple<object, object>(tbLogin.Text, pbPassword.Password));
        }
        private void DisplayNotification(Brush brush, string messege)
        {
            borderLogin.BorderBrush = brush;
            errorTextBlock.Foreground = brush;
            errorWrite.Visibility = Visibility.Visible;
            errorTextBlock.Text = messege;
        }

        private void btnCome_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(pbPassword.Password) || string.IsNullOrEmpty(tbLogin.Text))
            {
                DisplayNotification(Brushes.Red, "Введите логин и пароль");
                borderPassword.BorderBrush = Brushes.Red;
                return;
            }
            else ViewModel.AuthorizationCommand.Execute(new Tuple<object, object>(tbLogin.Text, pbPassword.Password));
        }
       
    }
}
