using System.ComponentModel;
using System.Windows;
using Atarashii.GUI;

namespace Atarashii.Loader.GUI
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
            _main.LogWindow.InitialiseFor(this);
        }

        private void Load(object sender, RoutedEventArgs e)
        {
            _main.Load();
        }

        private void Browse(object sender, RoutedEventArgs e)
        {
            _main.HcePath = BaseModel.PickFile("HCE Executable|haloce.exe");
        }

        private void Detect(object sender, RoutedEventArgs e)
        {
            _main.DetectExecutablePath();
        }

        private void Copy(object sender, RoutedEventArgs e)
        {
            _main.CopyToClipboard(_main.HcePath);
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            BaseModel.Exit();
        }
    }
}