using Labb03_GUI.API;
using Labb03_GUI.Command;
using Labb03_GUI.Dialogs;
using Labb03_GUI.Models;
using Labb03_GUI.Views;
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
        public DelegateCommand? RemoveQuestionCommand { get; }
        public DelegateCommand SetActivePackCommand { get;  }

        private bool _hasActivePack;
        public bool HasActivePack
        {
            get => _hasActivePack;
            set
            {
                _hasActivePack = value;
                RaisePropertyChanged();
            }
        }
        public MenuViewModel(MainWindowViewModel mainWindowViewModel)
        {
            this._mainWindowViewModel = mainWindowViewModel;
            OpenOptionsDialogCommand = new DelegateCommand(OpenOptionsDialog, CanOpenOption);
            DeleteActivePackCommand = new DelegateCommand(DeleteActivePack, CanDeleteActivePack);
            OpenCreateDialogCommand = new DelegateCommand(OpenCreateDialog, CanOpenCreate);
            OpenImportDialogCommand = new DelegateCommand(OpenImportDialog, CanOpenImportDialog);
            SetActivePackCommand = new DelegateCommand(SetActivePack, CanSetActivePack);
            HasActivePack = true;
        }

        private bool CanSetActivePack(object? arg)
        {
                return _mainWindowViewModel?.CurrentView != _mainWindowViewModel?.PlayerView
                    && _mainWindowViewModel?.CurrentView != _mainWindowViewModel?.PlayerEndScreenView;
        }
        private void SetActivePack(object? obj)
        {
            if (_mainWindowViewModel != null)
            {
                if (obj is QuestionPackViewModel selectedPack)
                {
                    _mainWindowViewModel.ActivePack = selectedPack;
                }
            }
        }

        private bool CanOpenImportDialog(object? arg)
        {
            return _mainWindowViewModel?.ActivePack != null
                && _mainWindowViewModel.CurrentView != _mainWindowViewModel.PlayerView
                && _mainWindowViewModel.CurrentView != _mainWindowViewModel.PlayerEndScreenView; ;
        }
        private async void OpenImportDialog(object? obj)
        {
            APIService? _aPIService = new APIService();
            if (await _aPIService.IsServerAvailable())
            {
                var dialog = new ImportQuestionsDialog();
                _mainWindowViewModel?.ImportQuestionsDialogViewModel.LoadCategoriesAsync();
                dialog.Owner = Application.Current.MainWindow;
                dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                dialog.DataContext = _mainWindowViewModel?.ImportQuestionsDialogViewModel;
                dialog.ShowDialog();
            }
            else
            {
                MessageBox.Show("No host available, check your network connection", "Unable to reach server", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private bool CanDeleteActivePack(object? arg)
        {
            return _mainWindowViewModel?.ActivePack != null 
                && _mainWindowViewModel.CurrentView != _mainWindowViewModel.PlayerView 
                && _mainWindowViewModel.CurrentView != _mainWindowViewModel.PlayerEndScreenView;
        }
        private void DeleteActivePack(object? obj)
        {
            if (_mainWindowViewModel != null 
                && _mainWindowViewModel.ActivePack != null
                && _mainWindowViewModel.Packs != null)
            {
                var result = MessageBox.Show(
                    $"Are you sure you want to delete '{_mainWindowViewModel.ActivePack.Name}'?",
                    "Confirm removal",
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

        private bool CanOpenCreate(object? arg)
        {
            return _mainWindowViewModel?.CurrentView != _mainWindowViewModel?.PlayerView
                && _mainWindowViewModel?.CurrentView != _mainWindowViewModel?.PlayerEndScreenView;
        }
        private void OpenCreateDialog(object? obj)
        {
            var dialog = new CreateNewPackDialog();
            dialog.Owner = Application.Current.MainWindow;
            dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            dialog.DataContext = _mainWindowViewModel?.CreateNewPackDialogViewModel;
            dialog.ShowDialog();
        }

        private bool CanOpenOption(object? arg)
        {
            return _mainWindowViewModel?.ActivePack != null
                && _mainWindowViewModel.CurrentView != _mainWindowViewModel.PlayerView
                && _mainWindowViewModel.CurrentView != _mainWindowViewModel.PlayerEndScreenView;
        }
        private void OpenOptionsDialog(object? obj)
        {
            var dialog = new PackOptionsDialog();
            dialog.Owner = Application.Current.MainWindow;
            dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            dialog.DataContext = _mainWindowViewModel?.PackOptionsDialogViewModel;
            dialog.ShowDialog();
        }
    }
}
