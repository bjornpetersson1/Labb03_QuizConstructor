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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

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
                _mainWindowViewModel.CurrentView = _mainWindowViewModel.ConfigurationView;
            };
            Closing += MainWindow_Closing;
        }

        private async void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var myPacks = _mainWindowViewModel.Packs.Select(ps => ps.GetModel()).ToList();
            await _jsonModel.SaveToJsonAsync(myPacks);
        }
        private void ToggleFullscreen_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.F11)
            {
                Window window = Window.GetWindow(this);
                if (window == null) return;
                if (window.WindowStyle != WindowStyle.None)
                {
                    window.WindowStyle = WindowStyle.None;
                    window.WindowState = WindowState.Maximized;
                    window.ResizeMode = ResizeMode.CanResize;
                }
                else
                {
                    window.WindowState = WindowState.Normal;
                    window.WindowStyle = WindowStyle.SingleBorderWindow;
                    window.ResizeMode = ResizeMode.CanResize;
                }
            }
        }
    }
}