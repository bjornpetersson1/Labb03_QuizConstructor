using Labb03_GUI.Dialogs;
using Labb03_GUI.Models;
using Labb03_GUI.ViewModels;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Labb03_GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowViewModel _mainWindowViewModel;
        private readonly JsonModel _jsonModel = new JsonModel();
        public MainWindow()
        {
            InitializeComponent();
            _mainWindowViewModel = new MainWindowViewModel();
            DataContext = _mainWindowViewModel;

            Loaded += async (s, e) =>
            {
                await _mainWindowViewModel.IntializeAsync();
            };
            Closing += MainWindow_Closing;
        }

        private async void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var myPacks = _mainWindowViewModel.Packs.Select(ps => ps.GetModel()).ToList();
            await _jsonModel.SaveToJsonAsync(myPacks);
        }
    }
}