using AggregatorBankingServices.ExpertSystem.Interfaces;
using AggregatorBankingServices.ExpertSystem.Repository;
using AggregatorBankingServices.ExpertSystem;

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
using AggregatorBankingServices.KnowledgeBase;
using System.Collections.ObjectModel;
using AggregatorBankingServices.Models;

namespace AggregatorBankingServices.View
{

    public record ClientAnswer(string Question, string SelectedAnswer) : IClientAnswer;

    public partial class Testing : Window
    {

        public string Question { get; set; }
        string replay { get; set; }

        private class MenuItem
        {
            public MenuItem()
            {
                this.Items = new ObservableCollection<MenuItem>();
            }
            public string Title { get; set; }

            public ObservableCollection<MenuItem> Items { get; set; }
        }

        IExpertSystemRepositoty repository;
        ExpertSystem.ExpertSystem expert_system;
        IExpertSystemResponse response;

        public Testing()
        {
            InitializeComponent();
            repository = new EFKnowledgeBase();
            expert_system = new ExpertSystem.ExpertSystem(repository);
            response = expert_system.GetResponse();

            UserTesting(response);
        }

     

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var value = sender as Button;
            replay = (string)value.Content;

            var answer = new ClientAnswer(Question, replay);

            UserTesting(expert_system.GetResponse(answer));
        }

        private void UserTesting(IExpertSystemResponse response)
        {
            grid_question.Children.Clear();
            grid_variable.Children.Clear();
            grid_variable.RowDefinitions.Clear();

            TextBlock textBlock = CreateTextBlock();

            if (response.OutputMachineResponse.Question is null)
            {
                ViewModel.ChangeScoring.Execute(new Tuple<object, object>(ViewModel.User.Login, response.OutputMachineResponse.Conclusion));
                TreeViewFilling(response);
                return;
            }

            Question = response.OutputMachineResponse.Question;

            textBlock.Text = response.OutputMachineResponse.Question;
            grid_question.Children.Add(textBlock);

            var options = response.OutputMachineResponse.QuestionOptions.ToList();

            for (int i = 0; i < options.Count; i++)
            {
                RowDefinition rowDefinition = new RowDefinition();

                Button button = CreateButton();
                button.Content = options[i];
                button.Click += Button_Click;

                grid_variable.RowDefinitions.Add(rowDefinition);

                Grid.SetRow(button, i);

                grid_variable.Children.Add(button);
            }

        }

        private void TreeViewFilling(IExpertSystemResponse response)
        {
            foreach (var explanation in response.ExplanatoryMachineResponse.Explanations)
            {
                MenuItem root = new MenuItem() { Title = $"Правило: {explanation.RuleName}" };

                MenuItem childItem1 = new MenuItem() { Title = "Если" };

                foreach (var fact in explanation.TrueFacts)
                {
                    childItem1.Items.Add(new MenuItem() { Title = $"{fact.VariableName} = {fact.VariableValue}" });
                }
                root.Items.Add(childItem1);
                root.Items.Add(new MenuItem() { Title = "То: ", Items = new ObservableCollection<MenuItem>() { new MenuItem() { Title = $"{explanation.ResultingFact.VariableName} = {explanation.ResultingFact.VariableValue}" } } });
                root.Items.Add(new MenuItem() { Title = "Так как: ", Items = new ObservableCollection<MenuItem>() { new MenuItem() { Title = $"{explanation.ExplanationText}" } } });

                tree_view.Items.Add(root);
            }
        }

        private TextBlock CreateTextBlock()
        {
            TextBlock textBlock = new TextBlock();
            textBlock.TextWrapping = TextWrapping.Wrap;
            textBlock.FontSize = 20;
            return textBlock;

        }
        private Button CreateButton()
        {
            Button button = new Button();
            button.Margin = new Thickness(10, 0, 10, 10);
            return button;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }
    }

}
