using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb03_GUI.ViewModels
{
    class CreateNewPackDialogViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel _mainWindowViewModel;
        public CreateNewPackDialogViewModel(MainWindowViewModel mainWindowViewModel)
        {
            this._mainWindowViewModel = mainWindowViewModel;
        }
    }
}
