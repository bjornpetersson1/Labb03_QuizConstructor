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
        private readonly Random random = new Random();
        DispatcherTimer timer = new DispatcherTimer();
        public ObservableCollection<AnswerViewModel> AnswerViewModels { get; set; } = new ObservableCollection<AnswerViewModel>();
        public QuestionPackViewModel? ActivePack { get => _mainWindowViewModel?.ActivePack; }
        public List<Question> RandomQuestions { get; set; } = new List<Question>();
        public ObservableCollection<string> Answers { get; set; } = new ObservableCollection<string>();
        public DelegateCommand CheckAnswerCommand { get; }

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

        private bool _isAnswerCorrectVisible;
        public bool IsAnswerCorrectVisible
        {
            get => _isAnswerCorrectVisible;
            set
            {
                _isAnswerCorrectVisible = value;
                RaisePropertyChanged();
            }
        }

        private bool _isAnswerIncorrectVisible;
        public bool IsAnswerIncorrectVisible
        {
            get => _isAnswerIncorrectVisible;
            set
            {
                _isAnswerIncorrectVisible = value;
                RaisePropertyChanged();
            }
        }

        private int _timeLeft;
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
            if (obj is not AnswerViewModel answer || CurrentQuestion == null)
                return;

            HandleAnswerResult(answer);
            await ProceedToNextQuestionOrEndAsync();
        }

        private void HandleAnswerResult(AnswerViewModel answer)
        {
            foreach (var ans in AnswerViewModels)
                ans.IsSelected = false;

            answer.IsSelected = true;

            answer.IsCorrect = answer.Text == CurrentQuestion?.CorrectAnswer;

            if ((bool)answer.IsCorrect)
            {
                NumberOfCorrectAnswers++;
                IsAnswerCorrectVisible = true;
                IsAnswerIncorrectVisible = false;
            }
            else
            {
                IsAnswerCorrectVisible = false;
                IsAnswerIncorrectVisible = true;
            }

            foreach (var ans in AnswerViewModels)
                ans.IsCorrect = ans.Text == CurrentQuestion?.CorrectAnswer;
        }

        private async Task ProceedToNextQuestionOrEndAsync()
        {
            timer.Stop();
            await Task.Delay(2000);
            IsAnswerCorrectVisible = false;
            IsAnswerIncorrectVisible = false;
            if (CurrentQuestionIndex < RandomQuestions.Count - 1)
                LoadNextQuestion();
            else
                EndQuiz();
        }

        private void LoadNextQuestion()
        {
            if (ActivePack != null)
            {
                CurrentQuestionIndex++;
                RandomiseActiveQuestionAnswers(CurrentQuestionIndex);
                TimeLeft = ActivePack.TimeLimitInSeconds;
                timer.Start();
                NumberOfCurrentQuestion++;
            }
        }

        private void EndQuiz()
        {
            _mainWindowViewModel?.OpenEndScreenCommand.Execute(null);
        }

        private async void Timer_Tick(object? sender, EventArgs e)
        {
            TimeLeft--;
            if (TimeLeft <= 0)
            {
                timer.Stop();
                await HandleTimeoutAsync();
            }
        }

        private async Task HandleTimeoutAsync()
        {
            foreach (var ans in AnswerViewModels)
            {
                if (ans.Text == CurrentQuestion?.CorrectAnswer)
                    ans.IsCorrect = true;
                else
                    ans.IsCorrect = false;
            }

            IsAnswerCorrectVisible = false;
            IsAnswerIncorrectVisible = true;

            await ProceedToNextQuestionOrEndAsync();
        }

        public void SetAndStartTimer()
        {
            if (ActivePack != null)
            {
                TimeLeft = ActivePack.TimeLimitInSeconds;
                timer.Start();
            }
        }

        public void ResetGame()
        {
            NumberOfCorrectAnswers = 0;
            NumberOfCurrentQuestion = 1;
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
