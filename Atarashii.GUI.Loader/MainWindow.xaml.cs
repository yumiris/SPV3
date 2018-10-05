using System.Windows;
using Microsoft.Win32;

namespace Atarashii.GUI.Loader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly Main _main;

        public MainWindow()
        {
            InitializeComponent();
            _main = (Main) DataContext;
        }

        private void Load(object sender, RoutedEventArgs e) => _main.Load();

        private void Browse(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                _main.HcePath = openFileDialog.FileName;
            }
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e) => _main.AttemptDetection();
    }
}