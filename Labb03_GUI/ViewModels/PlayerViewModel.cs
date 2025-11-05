using Labb03_GUI.Command;
using Labb03_GUI.Models;

namespace Labb03_GUI.ViewModels
{
    class PlayerViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel? _mainWindowViewModel;
        public QuestionPackViewModel? ActivePack { get => _mainWindowViewModel?.ActivePack; }
        private readonly Random random = new Random();
        private List<Question> randomQuestions;

        public PlayerViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            this._mainWindowViewModel = mainWindowViewModel;
            randomQuestions = ActivePack?.Questions.ToList() ?? new List<Question>();
            randomQuestions = randomQuestions.OrderBy(q => random.Next()).ToList();
        }
    }
}
