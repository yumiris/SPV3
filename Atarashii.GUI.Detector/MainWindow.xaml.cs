using System.ComponentModel;
using System.Windows;

namespace Atarashii.GUI.Detector
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly Main _main;

        public MainWindow()
        {
            InitializeComponent();
            _main = (Main) DataContext;
            BaseApplication.Initialise(this, _main);
        }

        private void Detect(object sender, RoutedEventArgs e)
        {
            _main.DetectExecutablePath();
        }

        private void Copy(object sender, RoutedEventArgs e)
        {
            _main.CopyToClipboard();
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            BaseApplication.Exit();
        }
    }
}