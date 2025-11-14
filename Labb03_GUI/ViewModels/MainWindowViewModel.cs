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
        public ConfigurationView ConfigurationView { get; }
        public PlayerView PlayerView { get; }
        public PlayerEndScreenView PlayerEndScreenView { get; }
        public AnswerViewModel? AnswerViewModel { get; set; }
        public PlayerViewModel? PlayerViewModel { get; set; }
        public MenuViewModel? MenuViewModel { get; set; }
        public ConfigurationViewModel? ConfigurationViewModel { get; set; }
        public PackOptionsDialogViewModel? PackOptionsDialogViewModel { get; set; }
        public CreateNewPackDialogViewModel? CreateNewPackDialogViewModel { get; set; }
        public QuestionPackViewModel? QuestionPackViewModel { get; set; }
        public ImportQuestionsDialogViewModel ImportQuestionsDialogViewModel { get; }
        private JsonModel _jsonModel = new JsonModel();
        public DelegateCommand OpenPlayerViewCommand { get; }
        public DelegateCommand OpenConfigViewCommand { get; }
        public DelegateCommand OpenEndScreenCommand { get; }
        public DelegateCommand? ToggleFullscreenCommand { get; }
        public DelegateCommand ExitApplicationCommand { get; }

        private ObservableCollection<QuestionPackViewModel>? _packs;
        public ObservableCollection<QuestionPackViewModel>? Packs
        {
            get => _packs;
            set
            {
                _packs = value;
                RaisePropertyChanged(nameof(Packs));
                MenuViewModel?.DeleteActivePackCommand.RaiseCanExecuteChanged();
                MenuViewModel?.OpenOptionsDialogCommand.RaiseCanExecuteChanged();
                OpenConfigViewCommand.RaiseCanExecuteChanged();
            }
        }

        private UserControl? _currentView;
        public UserControl? CurrentView
        {
            get => _currentView;
            set 
            {
                _currentView = value;
                RaisePropertyChanged();
                MenuViewModel?.OpenOptionsDialogCommand.RaiseCanExecuteChanged();
                MenuViewModel?.DeleteActivePackCommand.RaiseCanExecuteChanged();
                MenuViewModel?.OpenCreateDialogCommand.RaiseCanExecuteChanged();
                MenuViewModel?.OpenImportDialogCommand.RaiseCanExecuteChanged();
                MenuViewModel?.SetActivePackCommand.RaiseCanExecuteChanged();
                ConfigurationViewModel?.AddQuestionCommand.RaiseCanExecuteChanged();
                ConfigurationViewModel?.RemoveQuestionCommand.RaiseCanExecuteChanged();
                OpenPlayerViewCommand.RaiseCanExecuteChanged();
                OpenConfigViewCommand.RaiseCanExecuteChanged();
            }
        }


        private QuestionPackViewModel? _activePack;
        public QuestionPackViewModel? ActivePack
        {
            get { return _activePack; }
            set
            {
                _activePack = value;
                if (Packs != null)
                {
                    foreach (var pack in Packs)
                    {
                        pack.RefreshActiveStatus();
                    }
                }
                RaisePropertyChanged();
                UpdateHasActivePack();
                ConfigurationViewModel?.AddQuestionCommand.RaiseCanExecuteChanged();
                PlayerViewModel?.RaisePropertyChanged(nameof(PlayerViewModel.ActivePack));
                PackOptionsDialogViewModel?.RaisePropertyChanged(nameof(PackOptionsDialogViewModel.ActivePack));
                MenuViewModel?.OpenOptionsDialogCommand.RaiseCanExecuteChanged();
                OpenPlayerViewCommand.RaiseCanExecuteChanged();
                MenuViewModel?.DeleteActivePackCommand.RaiseCanExecuteChanged();
                MenuViewModel?.OpenImportDialogCommand.RaiseCanExecuteChanged();
            }
        }
        public MainWindowViewModel()
        {
            MenuViewModel = new MenuViewModel(this);
            ConfigurationViewModel = new ConfigurationViewModel(this);
            PackOptionsDialogViewModel = new PackOptionsDialogViewModel(this);
            CreateNewPackDialogViewModel = new CreateNewPackDialogViewModel(this);
            PlayerViewModel = new PlayerViewModel(this);
            ImportQuestionsDialogViewModel = new ImportQuestionsDialogViewModel(this);
            ConfigurationView = new Views.ConfigurationView();
            PlayerView = new Views.PlayerView();
            PlayerEndScreenView = new Views.PlayerEndScreenView();
            OpenPlayerViewCommand = new DelegateCommand(OpenPlayerView, CanOpenPlayerView);
            OpenConfigViewCommand = new DelegateCommand(OpenConfigView, CanOpenConfigView);
            OpenEndScreenCommand = new DelegateCommand(OpenEndScreen);
            ExitApplicationCommand = new DelegateCommand(ExitApplication);
            Packs = new ObservableCollection<QuestionPackViewModel>();
            PlayerView.DataContext = PlayerViewModel;
        }

        private void ExitApplication(object? obj)
        {
            Application.Current.Shutdown();
        }

        private void OpenEndScreen(object? obj)
        {
            CurrentView = PlayerEndScreenView;
        }

        private bool CanOpenConfigView(object? arg)
        {
            return Packs?.Count != 0
                && CurrentView != ConfigurationView;
        }

        private void OpenConfigView(object? obj)
        {
            CurrentView = ConfigurationView;
        }
        private bool CanOpenPlayerView(object? arg)
        {
            return ActivePack != null
                 && ActivePack.Questions.Count > 0
                 && CurrentView != PlayerView;
        }
        private void OpenPlayerView(object? obj)
        {
            PlayerViewModel?.RandomiseActivePack();
            PlayerViewModel?.RandomiseActiveQuestionAnswers(PlayerViewModel.CurrentQuestionIndex);
            PlayerViewModel?.ResetGame();
            PlayerViewModel?.SetAndStartTimer();
            CurrentView = PlayerView;
        }

        public async Task IntializeAsync()
        {
            var loadedPacks = await _jsonModel.LoadFromJsonAsync();
            Packs?.Clear();
            foreach (var pack in loadedPacks)
            {
                Packs?.Add(new QuestionPackViewModel(pack, this));
            }
            if (Packs?.Count == 0)
            {
                var pack = new QuestionPack("MyNewQuestionPack");
                ActivePack = new QuestionPackViewModel(pack, this);
                Packs.Add(ActivePack);
            }
            else ActivePack = Packs?.FirstOrDefault();
        }
        private void UpdateHasActivePack()
        {
            if (MenuViewModel != null)
            {
                MenuViewModel.HasActivePack = ActivePack != null;
            }
        }
    }
}
