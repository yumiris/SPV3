using System.ComponentModel;
using System.Windows;

namespace Atarashii.GUI.Executable
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
            _main.ShowLogWindow(this);
        }

        private void Load(object sender, RoutedEventArgs e)
        {
            _main.Load();
        }

        private void Browse(object sender, RoutedEventArgs e)
        {
            _main.HcePath = _main.PickFile("HCE Executable|haloce.exe");
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
            _main.Exit();
        }
    }
}