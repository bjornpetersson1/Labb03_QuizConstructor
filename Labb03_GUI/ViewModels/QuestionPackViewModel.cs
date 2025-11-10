using Labb03_GUI.Command;
using Labb03_GUI.Models;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Labb03_GUI.ViewModels
{
    class QuestionPackViewModel : ViewModelBase
    {
        private readonly QuestionPack _model;
        private readonly MainWindowViewModel? _mainWindowViewModel;
        public DelegateCommand AddQuestionCommand { get; }
        public DelegateCommand RemoveQuestionCommand { get; }

        private Question? _selectedQuestion;
        public Question? SelectedQuestion
        {
            get => _selectedQuestion;
            set
            {
                _selectedQuestion = value;
                RaisePropertyChanged();
                RemoveQuestionCommand.RaiseCanExecuteChanged();
            }
        }
        public QuestionPackViewModel(QuestionPack model, MainWindowViewModel mainWindowViewModel)
        {
            _model = model;
            _mainWindowViewModel = mainWindowViewModel;
            Questions = new ObservableCollection<Question>(_model.Questions);
            Questions.CollectionChanged += Questions_CollectionChanged;
            AddQuestionCommand = new DelegateCommand(AddQuestion, CanAddQuestion);
            RemoveQuestionCommand = new DelegateCommand(RemoveQuestion, CanRemoveQuestion);
        }


        private bool CanRemoveQuestion(object? arg)
        {
            return arg is Question;
        }

        private void RemoveQuestion(object? obj)
        {
            if (obj is Question question && Questions.Contains(question))
            {
                Questions.Remove(question);
            }
        }

        private bool CanAddQuestion(object? arg)
        {
            return _mainWindowViewModel?.Packs.Count > 0 && _model.Questions != null;
        }

        private void AddQuestion(object? obj)
        {
            if (obj is Question question)
            {
                Questions.Add(question);
            }
            else
            {
                Questions.Add(new Question("<question>", "<correct answer>", "<incorrect answer>", "<incorrect answer>", "<incorrect answer>"));
            }
        }

        private void Questions_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add && e.NewItems != null)
            {
                foreach (Question q in e.NewItems) _model.Questions.Add(q);
            }
            if (e.Action == NotifyCollectionChangedAction.Remove && e.OldItems != null)
            {
                foreach (Question q in e.OldItems) _model.Questions.Remove(q);
            }
            if (e.Action == NotifyCollectionChangedAction.Replace && e.OldItems != null && e.NewItems != null)
            {
                _model.Questions[e.OldStartingIndex] = (Question)e.NewItems[0]!;
            }
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                _model.Questions.Clear();
            }
        }

        public Array Difficulties 
        {
            get => _model.Difficulties;  
        }
        public string Name
        {
            get => _model.Name;
            set
            {
                _model.Name = value;
                RaisePropertyChanged();
            }
        }
        public Difficulty Difficulty
        {
            get => _model.Difficulty;
            set
            {
                _model.Difficulty = value;
                RaisePropertyChanged();
            }
        }
        public int TimeLimitInSeconds
        {
            get => _model.TimeLimitInSeconds;
            set
            {
                _model.TimeLimitInSeconds = value;
                RaisePropertyChanged();
            }
        }
        public ObservableCollection<Question> Questions { get; set; }
        public QuestionPack GetModel()
        {
            _model.Questions = Questions.ToList();
            return _model;
        }
    }
}
