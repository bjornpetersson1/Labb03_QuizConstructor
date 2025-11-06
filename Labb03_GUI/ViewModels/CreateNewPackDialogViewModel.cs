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
        public QuestionPackViewModel TempNewPack { get; set; }
        private readonly QuestionPack _model;


        public CreateNewPackDialogViewModel(MainWindowViewModel mainWindowViewModel)
        {
            this._mainWindowViewModel = mainWindowViewModel;
            CreateNewPackCommand = new DelegateCommand(CreateNewPack);
            _model = new QuestionPack();
            TempNewPack = new QuestionPackViewModel(_model, mainWindowViewModel);
        }

        private void CreateNewPack(object? obj)
        {
            var newModel = new QuestionPack
            {
                Name = TempNewPack.Name,
                Difficulty = TempNewPack.Difficulty,
                TimeLimitInSeconds = TempNewPack.TimeLimitInSeconds
            };
            var newPack = new QuestionPackViewModel(newModel, _mainWindowViewModel);

            _mainWindowViewModel.Packs.Add(newPack);
            _mainWindowViewModel.ActivePack = newPack;
            TempNewPack = new QuestionPackViewModel(new QuestionPack(), _mainWindowViewModel);
            RaisePropertyChanged(nameof(TempNewPack));
            if (obj is System.Windows.Window window) window.Close();
        }
    }
}
