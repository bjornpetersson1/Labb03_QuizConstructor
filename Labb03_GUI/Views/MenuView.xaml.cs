using Labb03_GUI.Dialogs;
using Labb03_GUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Labb03_GUI.Views
{
    /// <summary>
    /// Interaction logic for MenuView.xaml
    /// </summary>
    public partial class MenuView : UserControl
    {
        public MenuView()
        {
            InitializeComponent();
        }

        private void Fullscreen_Click(object sender, RoutedEventArgs e)
        {
            Window window = Window.GetWindow(this);
            if (window.WindowState != WindowState.Maximized || window.WindowStyle != WindowStyle.None)
            {
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

        //private void OpenCreateDialog_Click(object sender, RoutedEventArgs e)
        //{
        //    var dialog = new CreateNewPackDialog();
        //    dialog.Owner = Application.Current.MainWindow;
        //    dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
        //    dialog.ShowDialog();
        //}
    }
}
