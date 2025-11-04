using Labb03_GUI.Command;
using Labb03_GUI.Dialogs;
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
        public DelegateCommand OpenPlayerViewCommand { get; }
        public DelegateCommand OpenConfigViewCommand { get; }
        public MenuViewModel(MainWindowViewModel mainWindowViewModel)
        {
            this._mainWindowViewModel = mainWindowViewModel;
            OpenOptionsDialogCommand = new DelegateCommand(OpenOptionsDialog);
            OpenCreateDialogCommand = new DelegateCommand(OpenCreateDialog);
            OpenPlayerViewCommand = new DelegateCommand(OpenPlayerView);
            OpenConfigViewCommand = new DelegateCommand(OpenConfigView);
        }

        private void OpenConfigView(object? obj)
        {
            _mainWindowViewModel.CurrentView = _mainWindowViewModel.ConfigurationView;
        }

        private void OpenPlayerView(object? obj)
        {
            _mainWindowViewModel.CurrentView = _mainWindowViewModel.PlayerView;
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
    }
}
