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
        public ObservableCollection<Question> Questions { get; set; }
        public bool IsActive => _mainWindowViewModel?.ActivePack == this;
        public void RefreshActiveStatus() => RaisePropertyChanged(nameof(IsActive));

        public QuestionPackViewModel(QuestionPack model, MainWindowViewModel mainWindowViewModel)
        {
            _model = model;
            _mainWindowViewModel = mainWindowViewModel;
            Questions = new ObservableCollection<Question>(_model.Questions);
            Questions.CollectionChanged += Questions_CollectionChanged;
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
            _mainWindowViewModel?.OpenPlayerViewCommand.RaiseCanExecuteChanged();
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

        public QuestionPack GetModel()
        {
            _model.Questions = Questions.ToList();
            return _model;
        }
    }
}
