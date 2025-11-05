using Labb03_GUI.Command;
using Labb03_GUI.Models;
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
        public DelegateCommand CreateNewPackCommand { get; }
        public QuestionPackViewModel tempNewPack { get; set; }
        private readonly QuestionPack _model;
        public CreateNewPackDialogViewModel(MainWindowViewModel mainWindowViewModel)
        {
            this._mainWindowViewModel = mainWindowViewModel;
            CreateNewPackCommand = new DelegateCommand(CreateNewPack);
            tempNewPack = new QuestionPackViewModel(_model);
        }

        private void CreateNewPack(object? obj)
        {
            
        }
    }
}
