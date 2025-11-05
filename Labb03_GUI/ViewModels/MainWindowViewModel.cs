using Labb03_GUI.Command;
using Labb03_GUI.Models;
using Labb03_GUI.Views;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace Labb03_GUI.ViewModels
{
    class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<QuestionPackViewModel> Packs { get; } = new();
        private QuestionPackViewModel _activePack;
        private UserControl _currentView;

        public UserControl CurrentView
        {
            get => _currentView;
            set 
            {
                _currentView = value;
                RaisePropertyChanged();
            }
        }


        public QuestionPackViewModel ActivePack
        {
            get { return _activePack; }
            set
            {
                _activePack = value;
                RaisePropertyChanged();
                PlayerViewModel?.RaisePropertyChanged(nameof(PlayerViewModel.ActivePack));
                PackOptionsDialogViewModel?.RaisePropertyChanged(nameof(PackOptionsDialogViewModel.ActivePack));
            }
        }
        public PlayerViewModel? PlayerViewModel { get; set; }
        public ConfigurationViewModel? ConfigurationViewModel { get; set; }
        public PackOptionsDialogViewModel? PackOptionsDialogViewModel { get; set; }
        public MenuViewModel? MenuViewModel { get; set; }
        public CreateNewPackDialogViewModel? CreateNewPackDialogViewModel { get; set; }
        public ConfigurationView ConfigurationView { get; }
        public PlayerView PlayerView { get; }
        public DelegateCommand OpenPlayerViewCommand { get; }
        public DelegateCommand OpenConfigViewCommand { get; }
        public MainWindowViewModel()
        {
            PlayerViewModel = new PlayerViewModel(this);
            ConfigurationViewModel = new ConfigurationViewModel(this);
            PackOptionsDialogViewModel = new PackOptionsDialogViewModel(this);
            MenuViewModel = new MenuViewModel(this);
            CreateNewPackDialogViewModel = new CreateNewPackDialogViewModel(this);
            ConfigurationView = new Views.ConfigurationView();
            PlayerView = new Views.PlayerView();
            CurrentView = ConfigurationView;
            OpenPlayerViewCommand = new DelegateCommand(OpenPlayerView);
            OpenConfigViewCommand = new DelegateCommand(OpenConfigView);

            var pack = new QuestionPack("MyQuestionPack");
            ActivePack = new QuestionPackViewModel(pack);
            ActivePack.Questions.Add(new Question("Vad heter du?", "mitt namn", "ditt namn", "era namn", "doms namn"));
            ActivePack.Questions.Add(new Question("Vad heter dom?", "doms namn", "ditt namn", "era namn", "mitt namn"));
            Packs.Add(ActivePack);

        }
        private void OpenConfigView(object? obj)
        {
            CurrentView = ConfigurationView;
        }

        private void OpenPlayerView(object? obj)
        {
            PlayerViewModel?.RandomiseActivePack();
            PlayerViewModel?.RandomiseActiveQuestionAnswers(0);
            CurrentView = PlayerView;
        }

    }
}
