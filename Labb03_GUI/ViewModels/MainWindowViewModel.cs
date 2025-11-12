using Labb03_GUI.Command;
using Labb03_GUI.Models;
using Labb03_GUI.Views;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Windows.Controls;

namespace Labb03_GUI.ViewModels
{
    class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<QuestionPackViewModel> Packs { get; } = new();
        private bool _isOnline;

        public bool IsOnline
        {
            get => _isOnline;
            set 
            {
                _isOnline = value;
                RaisePropertyChanged(nameof(IsOnline));
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
                ConfigurationViewModel.AddQuestionCommand.RaiseCanExecuteChanged();
                PlayerViewModel?.RaisePropertyChanged(nameof(PlayerViewModel.ActivePack));
                PackOptionsDialogViewModel?.RaisePropertyChanged(nameof(PackOptionsDialogViewModel.ActivePack));
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
        public ImportQuestionsDialogViewModel ImportQuestionsDialogViewModel { get; }
        private JsonModel _jsonModel = new JsonModel();
        public MainWindowViewModel()
        {
            ConfigurationViewModel = new ConfigurationViewModel(this);
            PackOptionsDialogViewModel = new PackOptionsDialogViewModel(this);
            MenuViewModel = new MenuViewModel(this);
            CreateNewPackDialogViewModel = new CreateNewPackDialogViewModel(this);
            ConfigurationView = new Views.ConfigurationView();
            PlayerView = new Views.PlayerView();
            PlayerEndScreen = new Views.PlayerEndScreen();
            CurrentView = ConfigurationView;
            OpenPlayerViewCommand = new DelegateCommand(OpenPlayerView, CanOpenPlayerView);
            OpenConfigViewCommand = new DelegateCommand(OpenConfigView, CanOpenConfigView);
            OpenEndScreenCommand = new DelegateCommand(OpenEndScreen);
            PlayerViewModel = new PlayerViewModel(this);
            PlayerView.DataContext = PlayerViewModel;
            ImportQuestionsDialogViewModel = new ImportQuestionsDialogViewModel(this);

            Packs.CollectionChanged += (s, e) =>
            { 
                RaisePropertyChanged(nameof(Packs));
                MenuViewModel?.RaisePropertyChanged(nameof(MenuViewModel.HasPacks));
                MenuViewModel?.DeleteActivePackCommand.RaiseCanExecuteChanged();
                MenuViewModel?.OpenOptionsDialogCommand.RaiseCanExecuteChanged();
                OpenConfigViewCommand.RaiseCanExecuteChanged();
                OpenPlayerViewCommand.RaiseCanExecuteChanged();
            };
            NetworkChange.NetworkAvailabilityChanged += (s, e) =>
            {
                IsOnline = e.IsAvailable;
            };
            UpdateInternetStatusAync();
        }
        private async Task UpdateInternetStatusAync()  // den här ska loopas med timer //eller kolla bara en gång när importfönstret öppnar
        {
            IsOnline = await CheckInternetConnectionActiveAsync();
        }
        public async Task<bool> CheckInternetConnectionActiveAsync()
        {
            try
            {
                using var client = new HttpClient(); //gör bara en
                client.Timeout = TimeSpan.FromSeconds(4);
                var respons = await client.GetAsync("https://github.com/everyloop");
                return respons.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
        public async Task IntializeAsync()
        {
            var loadedPacks = await _jsonModel.LoadFromJsonAsync();
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
            ActivePack = Packs.FirstOrDefault();
        }
        private void OpenEndScreen(object? obj)
        {
            CurrentView = PlayerEndScreen;
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
