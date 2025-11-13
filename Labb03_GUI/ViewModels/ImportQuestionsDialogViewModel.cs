using Labb03_GUI.API;
using Labb03_GUI.Command;
using Labb03_GUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Labb03_GUI.ViewModels
{
    internal class ImportQuestionsDialogViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel _mainWindowViewModel;
        private readonly APIService aPIService = new APIService();
        public string[] Difficulties { get; set; } = new string[] { "Easy", "Medium", "Hard" };
        public DelegateCommand ImportQuestionsCommand { get; }
        public List<Category> Categories { get; set; } = new List<Category>();

        private Category? _selectedCategory;
        public Category? SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                ImportQuestionModel.Category = value;
                RaisePropertyChanged(nameof(SelectedCategory));
            }
        }

        private int _numbersOfQuestions;
        public int NumberOfQuestions
        {
            get => _numbersOfQuestions;
            set
            {
                if (value < 1) _numbersOfQuestions = 1;
                else if (value > 20) _numbersOfQuestions = 20;
                else _numbersOfQuestions = value;
                RaisePropertyChanged();
            }
        }

        private ImportQuestion? _importQuestioModel;
        public ImportQuestion? ImportQuestionModel 
        {
            get => _importQuestioModel;
            set
            {
                _importQuestioModel = value;
                RaisePropertyChanged(nameof(ImportQuestionModel));
            }
        } 

        private string? _selectedDifficulty;
        public string? SelectedDifficulty
        {
            get => _selectedDifficulty; 
            set 
            {
                _selectedDifficulty = value;
                if (ImportQuestionModel != null)
                ImportQuestionModel.Difficulty = value;
                RaisePropertyChanged(nameof(SelectedDifficulty));

            }
        }

        public ImportQuestionsDialogViewModel(MainWindowViewModel mainWindowViewModel)
        {
            _mainWindowViewModel = mainWindowViewModel;
            ImportQuestionModel = new ImportQuestion();
            ImportQuestionsCommand = new DelegateCommand(async _ => ImportQuestionsAsync());
            NumberOfQuestions = 1;
            SelectedDifficulty = Difficulties[1];
        }

        public async void LoadCategoriesAsync()
        {
            Categories = await aPIService.GetCategoriesAsync();
            RaisePropertyChanged(nameof(Categories));

            if (Categories.Any())
            {
                SelectedCategory = Categories.OrderBy(c => c.Id).First();
            }
        }

        private async Task ImportQuestionsAsync()
        {
            var (questions, responseCode) = await aPIService.GetQuestionsAsync(NumberOfQuestions, ImportQuestionModel);
            foreach (var que in questions)
            {
                string decodedQuery = WebUtility.HtmlDecode(que.Question);
                string decodedCorrectAnswer = WebUtility.HtmlDecode(que.Correct_Answer);
                var decodedIncorrectAnswers = que.Incorrect_Answers.Select(ans => WebUtility.HtmlDecode(ans)).ToList();
                _mainWindowViewModel.ConfigurationViewModel?.AddQuestionCommand.Execute(
                    new Question(decodedQuery, decodedCorrectAnswer, decodedIncorrectAnswers[0], decodedIncorrectAnswers[1], decodedIncorrectAnswers[2]));
            }
            string responseMessage = TokenRespons.TokenMessage.ContainsKey(responseCode) ? TokenRespons.TokenMessage[responseCode] : "Unknown error occured";
            MessageBox.Show(responseMessage, "Question import status", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
