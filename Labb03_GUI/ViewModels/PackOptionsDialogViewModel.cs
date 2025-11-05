using Labb03_GUI.Command;
using Labb03_GUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb03_GUI.ViewModels
{
    class PackOptionsDialogViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel? _mainWindowViewModel;
        public Array Difficulties => ActivePack.Difficulties;

        public DelegateCommand SetPackNameCommand { get; }
        public DelegateCommand SetDiffucultyCommand { get; }
        public QuestionPackViewModel? ActivePack { get => _mainWindowViewModel?.ActivePack; }
        
        private string _packName = string.Empty;

        public string PackName
        {
            get => _packName;
            set 
            {
                if (_packName != value)
                {
                    _packName = value;
                    RaisePropertyChanged();
                }
            }
        }


        public PackOptionsDialogViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            this._mainWindowViewModel = mainWindowViewModel;
        }
    }
}
