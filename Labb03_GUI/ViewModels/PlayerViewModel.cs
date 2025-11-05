using Labb03_GUI.Command;
using Labb03_GUI.Models;
using System.Windows.Threading;

namespace Labb03_GUI.ViewModels
{
    class PlayerViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel? _mainWindowViewModel;
        private int _timeLeft;
        public QuestionPackViewModel? ActivePack { get => _mainWindowViewModel?.ActivePack; }
        private readonly Random random = new Random();
        public List<Question> RandomQuestions { get; set; } = new List<Question>();
        public List<string> Answers { get; set; } = new List<string>();
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

        public PlayerViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            this._mainWindowViewModel = mainWindowViewModel;
            timer.Interval = TimeSpan.FromSeconds(1.0);
            timer.Tick += Timer_Tick;
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
            Answers = allAnswers.OrderBy(a => random.Next()).ToList();            
        }

    }
}
