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
        public QuestionPackViewModel QuestionPackViewModel { get; set; }
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
            OpenPlayerViewCommand = new DelegateCommand(OpenPlayerView, CanOpenPlayerView);
            OpenConfigViewCommand = new DelegateCommand(OpenConfigView, CanOpenConfigView);

            Packs.CollectionChanged += (s, e) =>
            { 
                RaisePropertyChanged(nameof(Packs));
                MenuViewModel?.DeleteActivePackCommand.RaiseCanExecuteChanged();
                MenuViewModel?.OpenOptionsDialogCommand.RaiseCanExecuteChanged();
                OpenConfigViewCommand.RaiseCanExecuteChanged();
                OpenPlayerViewCommand.RaiseCanExecuteChanged();
                foreach (var pack in Packs)
                {
                    pack.AddQuestionCommand.RaiseCanExecuteChanged();
                    pack.RemoveQuestionCommand.RaiseCanExecuteChanged();
                }
                ActivePack?.AddQuestionCommand.RaiseCanExecuteChanged();
                ActivePack?.RemoveQuestionCommand.RaiseCanExecuteChanged();
            };

            var pack = new QuestionPack("MyQuestionPack");
            ActivePack = new QuestionPackViewModel(pack, this);
            ActivePack.Questions.Add(new Question("Vad heter du?", "mitt namn", "ditt namn", "era namn", "doms namn"));
            ActivePack.Questions.Add(new Question("Vad heter dom?", "doms namn", "ditt namn", "era namn", "mitt namn"));
            ActivePack.Questions.Add(new Question("Hur mycke sover du?", "6 timmar", "4 timmar", "lagom", "för mycket"));
            ActivePack.Questions.Add(new Question("okej?", "JA!!!", "javiss", "t", "45"));
            ActivePack.Questions.Add(new Question("en till fråååååga!!", "23", "43", "67", "99"));
            Packs.Add(ActivePack);

        }

        private bool CanOpenConfigView(object? arg)
        {
            return Packs.Count != 0;
        }

        private bool CanOpenPlayerView(object? arg)
        {
           return Packs.Count > 0 && ActivePack != null && ActivePack.Questions.Count > 0;
        }

        private void OpenConfigView(object? obj)
        {
            CurrentView = ConfigurationView;
        }

        private void OpenPlayerView(object? obj)
        {
            PlayerViewModel?.RandomiseActivePack();
            PlayerViewModel?.RandomiseActiveQuestionAnswers(PlayerViewModel.CurrentQuestionIndex);
            CurrentView = PlayerView;
        }

    }
}
