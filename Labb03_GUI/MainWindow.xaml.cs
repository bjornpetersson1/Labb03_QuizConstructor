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
        private int count = 1;
        public MainWindow()
        {
            InitializeComponent();
            var pack = new QuestionPack("MyPack");
            DataContext = new QuestionPackViewModel(pack);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            QuestionPackViewModel viewModel = (DataContext as QuestionPackViewModel);
            PackOptionsDialog myWindow = new PackOptionsDialog();
            viewModel.Name = "New name";
            viewModel.Questions.Add(new Question($"{count++}VAd är gult?", "1", "2", "3", "4"));
            myWindow.Show();
        }
    }
}