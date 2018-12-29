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
    }
}