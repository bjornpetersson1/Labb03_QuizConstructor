using Labb03_GUI.Command;
using Labb03_GUI.Models;
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
        public QuestionPackViewModel? ActivePack { get => _mainWindowViewModel?.ActivePack; }
        private readonly Random random = new Random();
        public List<Question> RandomQuestions { get; set; } = new List<Question>();
        public ObservableCollection<string> Answers { get; set; } = new ObservableCollection<string>();
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
            CheckAnswerCommand = new DelegateCommand(CheckAnswer);
        }

        private void CheckAnswer(object? obj)
        {
            if (CurrentQuestion == null) return;

            if (obj == CurrentQuestion.CorrectAnswer)
            {
                MessageBox.Show("Rätt svar! 🎉");
            }
            else
            {
                MessageBox.Show($"Fel svar! 😢 Rätt svar var: {CurrentQuestion.CorrectAnswer}");
            }
            if (CurrentQuestionIndex < RandomQuestions.Count - 1)
            {
                CurrentQuestionIndex++;
                RandomiseActiveQuestionAnswers(CurrentQuestionIndex);
                TimeLeft = ActivePack.TimeLimitInSeconds;
            }
            else
            {
                timer.Stop();
                MessageBox.Show("Spelet är slut!");
            }
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            TimeLeft--;
        }

        public void RandomiseActivePack()
        {
            RandomQuestions = ActivePack?.Questions.ToList() ?? new List<Question>();
            RandomQuestions = RandomQuestions.OrderBy(q => random.Next()).ToList();
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
            var allAnswers = new List<string>();
            allAnswers.Add(currentQuestion.CorrectAnswer);
            allAnswers.Add(currentQuestion.IncorrectAnswers[0]);
            allAnswers.Add(currentQuestion.IncorrectAnswers[1]);
            allAnswers.Add(currentQuestion.IncorrectAnswers[2]);
            Answers.Clear();
            foreach (var ans in allAnswers.OrderBy(a => random.Next()))
            {
                Answers.Add(ans);
            }
        }

    }
}
