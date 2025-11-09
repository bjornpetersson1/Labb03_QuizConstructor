using Labb03_GUI.API;
using Labb03_GUI.Command;
using Labb03_GUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb03_GUI.ViewModels
{
    internal class ImportQuestionsDialogViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel _mainWindowViewModel;
        private readonly APIService aPIService = new APIService();
        public string[] Difficulties { get; set; } = new string[] { "Easy", "Medium", "Hard" };
        public DelegateCommand ImportQuestionsCommand { get; }
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
        public List<Category> Categories { get; set; } = new List<Category>();
        private ImportQuestion _importQuestioModel;
        public ImportQuestion ImportQuestionModel 
        {
            get => _importQuestioModel;
            set
            {
                _importQuestioModel = value;
                RaisePropertyChanged(nameof(ImportQuestionModel));
            }
        } 
        private Category _selectedCategory;
        public Category SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                ImportQuestionModel.Category = value;
                RaisePropertyChanged(nameof(SelectedCategory));
            }
        }
        private string _selectedDifficulty;

        public string SelectedDifficulty
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
            LoadCategoriesAsync();
        }

        private async void LoadCategoriesAsync()
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
            var questions = await aPIService.GetQuestionsAsync(NumberOfQuestions, ImportQuestionModel);
            foreach (var que in questions)
            {
                _mainWindowViewModel.ActivePack.AddQuestionCommand.Execute(new Question(que.Question, que.Correct_Answer, que.Incorrect_Answers[0], que.Incorrect_Answers[1], que.Incorrect_Answers[2]));

            }
        }
    }
}
