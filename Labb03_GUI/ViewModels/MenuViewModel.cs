using Labb03_GUI.Command;
using Labb03_GUI.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb03_GUI.ViewModels
{
    class MenuViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel? _mainWindowViewModel;
        public DelegateCommand OpenOptionsDialogCommand { get; }
        public DelegateCommand OpenCreateDialogCommand { get; }
        public MenuViewModel(MainWindowViewModel mainWindowViewModel)
        {
            this._mainWindowViewModel = mainWindowViewModel;
            OpenOptionsDialogCommand = new DelegateCommand(OpenOptionsDialog);
            OpenCreateDialogCommand = new DelegateCommand(OpenCreateDialog);
        }

        private void OpenCreateDialog(object? obj)
        {
            var dialog = new CreateNewPackDialog();
            dialog.DataContext = _mainWindowViewModel.CreateNewPackDialogViewModel;
            dialog.ShowDialog();
        }

        private void OpenOptionsDialog(object? obj)
        {
            var dialog = new PackOptionsDialog();
            dialog.DataContext = _mainWindowViewModel.PackOptionsDialogViewModel;
            dialog.ShowDialog();
        }
    }
}
