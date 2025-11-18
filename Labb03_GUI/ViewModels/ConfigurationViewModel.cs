using Labb03_GUI.API;
using Labb03_GUI.Command;
using Labb03_GUI.Models;

namespace Labb03_GUI.ViewModels
{
    class ConfigurationViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel _mainWindowViewModel;
        public DelegateCommand AddQuestionCommand { get; }
        public DelegateCommand RemoveQuestionCommand { get; }
        public bool CanEditQuestion => RemoveQuestionCommand.CanExecute(null);

        private Question? _selectedQuestion;
        public Question? SelectedQuestion
        {
            get => _selectedQuestion;
            set
            {
                _selectedQuestion = value;
                RaisePropertyChanged();
                RemoveQuestionCommand.RaiseCanExecuteChanged();
                RaisePropertyChanged(nameof(CanEditQuestion));
            }
        }

        public ConfigurationViewModel(MainWindowViewModel mainWindowViewModel)
        {
            this._mainWindowViewModel = mainWindowViewModel;
            AddQuestionCommand = new DelegateCommand(AddQuestion, CanAddQuestion);
            RemoveQuestionCommand = new DelegateCommand(RemoveQuestion, CanRemoveQuestion);

        }

        private bool CanRemoveQuestion(object? arg)
        {
            return SelectedQuestion != null && _mainWindowViewModel.ActivePack != null;
        }

        private void RemoveQuestion(object? obj)
        {
            if(_mainWindowViewModel.ActivePack != null && SelectedQuestion != null)
            {
                _mainWindowViewModel.ActivePack.Questions.Remove(SelectedQuestion);
            }
        }

        private bool CanAddQuestion(object? arg)
        {
            return _mainWindowViewModel?.ActivePack != null
                && _mainWindowViewModel.CurrentView != _mainWindowViewModel.PlayerView
                && _mainWindowViewModel.CurrentView != _mainWindowViewModel.PlayerEndScreenView;
        }

        private void AddQuestion(object? obj)
        {
            if (_mainWindowViewModel.ActivePack != null)
            {
                if (obj is Question question)
                {
                    _mainWindowViewModel.ActivePack.Questions.Add(question);
                }
                else
                {
                    _mainWindowViewModel.ActivePack.Questions.Add(new Question("<question>", "<correct answer>", "<incorrect answer>", "<incorrect answer>", "<incorrect answer>"));
                }
            }
        }
    }
}
