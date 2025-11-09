using Labb03_GUI.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb03_GUI.ViewModels
{
    internal class ImportQuestionsDialogViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel _mainWindowViewModel;
        private readonly APIService aPIService = new APIService();
        public List<Category> Categories { get; set; }
        public ImportQuestionsDialogViewModel(MainWindowViewModel mainWindowViewModel)
        {
            _mainWindowViewModel = mainWindowViewModel;
            Categories = aPIService.GetCategoriesAsync().GetAwaiter().GetResult();
        }
    }
}
