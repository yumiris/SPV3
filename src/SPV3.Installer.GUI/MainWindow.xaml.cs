using System.Windows;

namespace SPV3.Installer.GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private Main _main;
        
        public MainWindow()
        {
            InitializeComponent();
            _main = (Main) DataContext;
        }

        private void Install(object sender, RoutedEventArgs e)
        {
            _main.Install();
        }

        /// <summary>
        ///     Invokes directory picker.
        /// </summary>
        private void Browse(object sender, RoutedEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                dialog.ShowDialog();
                _main.Target = dialog.SelectedPath;
            }
        }
    }
}