using Labb03_GUI.Command;
using Labb03_GUI.Models;
using Labb03_GUI.Views;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Windows;
using System.Windows.Controls;

namespace Labb03_GUI.ViewModels
{
    class MainWindowViewModel : ViewModelBase
    {
        private ObservableCollection<QuestionPackViewModel> _packs;
        public ObservableCollection<QuestionPackViewModel> Packs
        {
            get => _packs;
            set
            {
                _packs = value;
                RaisePropertyChanged(nameof(Packs));
                //MenuViewModel?.RaisePropertyChanged(nameof(MenuViewModel.HasPacks));
                MenuViewModel?.DeleteActivePackCommand.RaiseCanExecuteChanged();
                MenuViewModel?.OpenOptionsDialogCommand.RaiseCanExecuteChanged();
                OpenConfigViewCommand.RaiseCanExecuteChanged();
                OpenPlayerViewCommand.RaiseCanExecuteChanged();
            }
        }

        private UserControl _currentView;
        public UserControl CurrentView
        {
            get => _currentView;
            set 
            {
                _currentView = value;
                RaisePropertyChanged();
                MenuViewModel?.OpenOptionsDialogCommand.RaiseCanExecuteChanged();
                MenuViewModel?.DeleteActivePackCommand.RaiseCanExecuteChanged();
                ConfigurationViewModel?.AddQuestionCommand.RaiseCanExecuteChanged();
                ConfigurationViewModel?.RemoveQuestionCommand.RaiseCanExecuteChanged();
            }
        }


        private QuestionPackViewModel _activePack;
        public QuestionPackViewModel ActivePack
        {
            get { return _activePack; }
            set
            {
                _activePack = value;
                foreach (var pack in Packs)
                {
                    pack.RefreshActiveStatus();
                }
                RaisePropertyChanged();
                ConfigurationViewModel?.AddQuestionCommand.RaiseCanExecuteChanged();
                PlayerViewModel?.RaisePropertyChanged(nameof(PlayerViewModel.ActivePack));
                PackOptionsDialogViewModel?.RaisePropertyChanged(nameof(PackOptionsDialogViewModel.ActivePack));
                MenuViewModel?.OpenOptionsDialogCommand.RaiseCanExecuteChanged();
                OpenPlayerViewCommand.RaiseCanExecuteChanged();
            }
        }
        public AnswerViewModel AnswerViewModel { get; set; }
        public PlayerViewModel? PlayerViewModel { get; set; }
        public ConfigurationViewModel? ConfigurationViewModel { get; set; }
        public PackOptionsDialogViewModel? PackOptionsDialogViewModel { get; set; }
        public MenuViewModel? MenuViewModel { get; set; }
        public CreateNewPackDialogViewModel? CreateNewPackDialogViewModel { get; set; }
        public ConfigurationView ConfigurationView { get; }
        public PlayerView PlayerView { get; }
        public PlayerEndScreen PlayerEndScreen { get; }
        public QuestionPackViewModel QuestionPackViewModel { get; set; }
        public DelegateCommand OpenPlayerViewCommand { get; }
        public DelegateCommand OpenConfigViewCommand { get; }
        public DelegateCommand OpenEndScreenCommand { get; }
        public DelegateCommand ToggleFullscreenCommand { get; }
        public ImportQuestionsDialogViewModel ImportQuestionsDialogViewModel { get; }
        private JsonModel _jsonModel = new JsonModel();
        public MainWindowViewModel()
        {
            MenuViewModel = new MenuViewModel(this);
            ConfigurationViewModel = new ConfigurationViewModel(this);
            PackOptionsDialogViewModel = new PackOptionsDialogViewModel(this);
            ConfigurationView = new Views.ConfigurationView();
            CreateNewPackDialogViewModel = new CreateNewPackDialogViewModel(this);
            PlayerView = new Views.PlayerView();
            PlayerEndScreen = new Views.PlayerEndScreen();
            OpenPlayerViewCommand = new DelegateCommand(OpenPlayerView, CanOpenPlayerView);
            OpenConfigViewCommand = new DelegateCommand(OpenConfigView, CanOpenConfigView);
            OpenEndScreenCommand = new DelegateCommand(OpenEndScreen);
            PlayerViewModel = new PlayerViewModel(this);
            ImportQuestionsDialogViewModel = new ImportQuestionsDialogViewModel(this);
            Packs = new ObservableCollection<QuestionPackViewModel>();
            PlayerView.DataContext = PlayerViewModel;
        }

        public async Task IntializeAsync()
        {
            var loadedPacks = await _jsonModel.LoadFromJsonAsync();
            Packs.Clear();
            foreach (var pack in loadedPacks)
            {
                Packs.Add(new QuestionPackViewModel(pack, this));
            }
            if (Packs.Count == 0)
            {
                var pack = new QuestionPack("MyNewQuestionPack");
                ActivePack = new QuestionPackViewModel(pack, this);
                Packs.Add(ActivePack);
            }
            else ActivePack = Packs.FirstOrDefault();
        }
        private void OpenEndScreen(object? obj)
        {
            CurrentView = PlayerEndScreen;
        }

        private bool CanOpenConfigView(object? arg)
        {
            return Packs.Count != 0;
        }

        private void OpenConfigView(object? obj)
        {
            CurrentView = ConfigurationView;
        }
        private void OpenPlayerView(object? obj)
        {
            PlayerViewModel?.RandomiseActivePack();
            PlayerViewModel?.RandomiseActiveQuestionAnswers(PlayerViewModel.CurrentQuestionIndex);
            PlayerViewModel?.SetAndStartTimer();
            CurrentView = PlayerView;
        }
        private bool CanOpenPlayerView(object? arg)
        {
           return Packs.Count > 0 && ActivePack != null && ActivePack.Questions.Count > 0;
        }
    }
}
