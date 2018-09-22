using System.Windows;
using Microsoft.Win32;

namespace YuMi.Bbkpify.GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Main main = new Main();

        public MainWindow()
        {
            DataContext = main;
            InitializeComponent();
        }

        private void Commit(object sender, RoutedEventArgs e) => main.Commit();
        private void Revert(object sender, RoutedEventArgs e) => main.Revert();
        private void LoadSapien(object sender, RoutedEventArgs e) => main.LoadSapien();

        private void ChoosePlaceholder(object sender, RoutedEventArgs e)
        {
            var placeholderDialog = new OpenFileDialog()
            {
                Filter = "Bitmap files (*.bitmap)|*.bitmap|All files (*.*)|*.*"
            };
            
            if (placeholderDialog.ShowDialog() == true)
            {
                main.Placeholder = placeholderDialog.FileName;
            }
        }

        private void ChooseDirectory(object sender, RoutedEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                dialog.ShowDialog();
                main.Directory = dialog.SelectedPath;
            }
        }
    }
}