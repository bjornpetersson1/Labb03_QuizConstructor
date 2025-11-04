using Labb03_GUI.Command;

namespace Labb03_GUI.ViewModels
{
    class PlayerViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel? _mainWindowViewModel;
        public QuestionPackViewModel? ActivePack { get => _mainWindowViewModel?.ActivePack; }

        public PlayerViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            this._mainWindowViewModel = mainWindowViewModel;
        }
    }
}
