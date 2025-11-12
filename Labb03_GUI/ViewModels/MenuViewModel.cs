using Labb03_GUI.API;
using Labb03_GUI.Command;
using Labb03_GUI.Dialogs;
using Labb03_GUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Labb03_GUI.ViewModels
{
    class MenuViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel? _mainWindowViewModel;
        public DelegateCommand OpenOptionsDialogCommand { get; }
        public DelegateCommand OpenCreateDialogCommand { get; }
        public DelegateCommand OpenImportDialogCommand { get; }
        public DelegateCommand DeleteActivePackCommand { get; }
        public DelegateCommand RemoveQuestionCommand { get; }
        public DelegateCommand SetActivePackCommand { get;  }
        public bool HasPacks => _mainWindowViewModel.Packs.Any();
        public MenuViewModel(MainWindowViewModel mainWindowViewModel)
        {
            this._mainWindowViewModel = mainWindowViewModel;
            OpenOptionsDialogCommand = new DelegateCommand(OpenOptionsDialog, CanOpenOption);
            OpenCreateDialogCommand = new DelegateCommand(OpenCreateDialog);
            DeleteActivePackCommand = new DelegateCommand(DeleteActivePack, CanDeleteActivePack);
            OpenImportDialogCommand = new DelegateCommand(OpenImportDialog);
            SetActivePackCommand = new DelegateCommand(SetActivePack);
        }

        private void SetActivePack(object? obj)
        {
            if (obj is QuestionPackViewModel selectedPack)
            {
                _mainWindowViewModel.ActivePack = selectedPack;
            }
        }

        private async void OpenImportDialog(object? obj)
        {
            APIService? _aPIService = new APIService();
            if (await _aPIService.IsServerAvailable())
            {
                var dialog = new ImportQuestionsDialog();
                _mainWindowViewModel.ImportQuestionsDialogViewModel.LoadCategoriesAsync();
                dialog.Owner = Application.Current.MainWindow;
                dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                dialog.DataContext = _mainWindowViewModel.ImportQuestionsDialogViewModel;
                dialog.ShowDialog();
            }
            else
            {
                MessageBox.Show("No host available, check your network connection", "Unable to reach server", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private bool CanDeleteActivePack(object? arg)
        {
            return _mainWindowViewModel.Packs.Count != 0;
        }

        private void DeleteActivePack(object? obj)
        {
            if (_mainWindowViewModel.ActivePack != null)
            {
                var result = MessageBox.Show(
                    $"Är du säker på att du vill ta bort '{_mainWindowViewModel.ActivePack.Name}'?",
                    "Bekräfta borttagning",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning
                );

                if (result == MessageBoxResult.Yes)
                {
                    _mainWindowViewModel.Packs.Remove(_mainWindowViewModel.ActivePack);
                    _mainWindowViewModel.ActivePack = _mainWindowViewModel.Packs.FirstOrDefault();                    
                }
            }
        }

        private void OpenCreateDialog(object? obj)
        {
            var dialog = new CreateNewPackDialog();
            dialog.Owner = Application.Current.MainWindow;
            dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            dialog.DataContext = _mainWindowViewModel.CreateNewPackDialogViewModel;
            dialog.ShowDialog();

        }

        private void OpenOptionsDialog(object? obj)
        {
            var dialog = new PackOptionsDialog();
            dialog.Owner = Application.Current.MainWindow;
            dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            dialog.DataContext = _mainWindowViewModel.PackOptionsDialogViewModel;
            dialog.ShowDialog();
        }
        private bool CanOpenOption(object? arg)
        {
            return _mainWindowViewModel.Packs.Count != 0;
        }
    }
}
