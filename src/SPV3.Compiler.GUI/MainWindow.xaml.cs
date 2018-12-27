using System;
using System.Windows;

namespace SPV3.Compiler.GUI
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Main _main;
        
        public MainWindow()
        {
            InitializeComponent();
            _main = (Main) DataContext;
        }

        private void BrowseSource(object sender, RoutedEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                dialog.ShowDialog();
                _main.Source = dialog.SelectedPath;
            }
        }

        private void BrowseTarget(object sender, RoutedEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                dialog.ShowDialog();
                _main.Target = dialog.SelectedPath;
            }
        }

        private void Create(object sender, RoutedEventArgs e)
        {
            try
            {
                _main.Create();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
    }
}