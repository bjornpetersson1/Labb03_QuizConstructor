using Labb03_GUI.Command;
using Labb03_GUI.Models;
using Labb03_GUI.Views;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Threading;

namespace Labb03_GUI.ViewModels
{
    class PlayerViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel? _mainWindowViewModel;
        private int _timeLeft;
        public DelegateCommand CheckAnswerCommand { get; }
        public ObservableCollection<AnswerViewModel> AnswerViewModels { get; set; } = new ObservableCollection<AnswerViewModel>();
        public QuestionPackViewModel? ActivePack { get => _mainWindowViewModel?.ActivePack; }
        private readonly Random random = new Random();
        public List<Question> RandomQuestions { get; set; } = new List<Question>();
        public ObservableCollection<string> Answers { get; set; } = new ObservableCollection<string>();
        private int _numberOfCurrentQuestion;
        public int NumberOfCurrentQuestion 
        {
            get => _numberOfCurrentQuestion; 
            set
            {
                _numberOfCurrentQuestion = value;
                RaisePropertyChanged();
            }
        }
        private int _numberOfCorrectAnswers;
        public int NumberOfCorrectAnswers 
        {
            get => _numberOfCorrectAnswers;
            set
            {
                _numberOfCorrectAnswers = value;
                RaisePropertyChanged();
            }
        }
        DispatcherTimer timer = new DispatcherTimer();
        public int TimeLeft
        {
            get => _timeLeft;
            set
            {
                _timeLeft = value;
                RaisePropertyChanged();
            }
        }
        public Question? CurrentQuestion
        {
            get
            {
                if (RandomQuestions == null || RandomQuestions.Count == 0)
                    return null;
                if (CurrentQuestionIndex < 0 || CurrentQuestionIndex >= RandomQuestions.Count)
                    return null;
                return RandomQuestions[CurrentQuestionIndex];
            }
        }
        private int _currentQuestionIndex;
        public int CurrentQuestionIndex
        {
            get => _currentQuestionIndex;
            set
            {
                _currentQuestionIndex = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(CurrentQuestion));
            }
        }

        public PlayerViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            this._mainWindowViewModel = mainWindowViewModel;
            timer.Interval = TimeSpan.FromSeconds(1.0);
            timer.Tick += Timer_Tick;
            CurrentQuestionIndex = 0;
            NumberOfCorrectAnswers = 0;
            NumberOfCurrentQuestion = 1;
            CheckAnswerCommand = new DelegateCommand(CheckAnswer);
            RaisePropertyChanged(nameof(CurrentQuestion));
        }

        private async void CheckAnswer(object? obj)
        {
            if (obj is not AnswerViewModel answer || CurrentQuestion == null) return;

            answer.IsCorrect = answer.Text == CurrentQuestion.CorrectAnswer;
            if ((bool)answer.IsCorrect)
            {
                NumberOfCorrectAnswers++;
            }
            foreach (var ans in AnswerViewModels)
            {
                if (ans != answer && ans.Text != CurrentQuestion.CorrectAnswer)
                    ans.IsCorrect = false;
            }

            if (CurrentQuestionIndex < RandomQuestions.Count - 1)
            {
                timer.Stop();
                await Task.Delay(2000);
                CurrentQuestionIndex++;
                RandomiseActiveQuestionAnswers(CurrentQuestionIndex);
                TimeLeft = ActivePack.TimeLimitInSeconds;
                timer.Start();
            }
            else
            {
                timer.Stop();
                await Task.Delay(2000);
                _mainWindowViewModel.OpenEndScreenCommand.Execute(null);
            }
            NumberOfCurrentQuestion++;
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            TimeLeft--;
        }

        public void RandomiseActivePack()
        {
            RandomQuestions = ActivePack?.Questions.ToList() ?? new List<Question>();
            RandomQuestions = RandomQuestions.OrderBy(q => random.Next()).ToList();

            if (RandomQuestions.Count > 0)
            {
                CurrentQuestionIndex = 0;
                RaisePropertyChanged(nameof(CurrentQuestion));
                RandomiseActiveQuestionAnswers(CurrentQuestionIndex);
            }

            if (ActivePack != null)
            {
                TimeLeft = ActivePack.TimeLimitInSeconds;
                timer.Start();
            }
        }
        public void RandomiseActiveQuestionAnswers(int questionIndex)
        {
            if (RandomQuestions == null || RandomQuestions.Count == 0) return;
            var currentQuestion = RandomQuestions[questionIndex];

            var allAnswers = new List<AnswerViewModel>
            {
                new AnswerViewModel(currentQuestion.CorrectAnswer),
                new AnswerViewModel(currentQuestion.IncorrectAnswers[0]),
                new AnswerViewModel(currentQuestion.IncorrectAnswers[1]),
                new AnswerViewModel(currentQuestion.IncorrectAnswers[2])
            };
            AnswerViewModels.Clear();
            foreach (var ans in allAnswers.OrderBy(a => random.Next()))
            {
                AnswerViewModels.Add(ans);
            }
        }

    }
}
