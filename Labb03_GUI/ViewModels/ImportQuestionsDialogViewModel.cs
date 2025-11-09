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
        public List<Category> Categories { get; set; } = new List<Category>();
        private Category _selectedCategory;
        public Category SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                RaisePropertyChanged(nameof(SelectedCategory));
            }
        }
        public ImportQuestionsDialogViewModel(MainWindowViewModel mainWindowViewModel)
        {
            _mainWindowViewModel = mainWindowViewModel;
            LoadCategoriesAsync();
        }
        private async void LoadCategoriesAsync()
        {
            Categories = await aPIService.GetCategoriesAsync();
            RaisePropertyChanged(nameof(Categories));

            if (Categories.Any())
            {
                SelectedCategory = Categories.OrderBy(c => c.Id).First();
            }
        }
    }
}
