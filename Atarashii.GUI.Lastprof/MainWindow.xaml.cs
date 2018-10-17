using System.ComponentModel;
using System.Windows;

namespace Atarashii.GUI.Lastprof
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

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            BaseModel.Exit();
        }

        private void Browse(object sender, RoutedEventArgs e)
        {
            _main.LastprofFile = BaseModel.PickFile("Lastprof File|lastprof.txt");
        }

        private void Copy(object sender, RoutedEventArgs e)
        {
            _main.CopyToClipboard(_main.LastprofFile);
        }

        private void Parse(object sender, RoutedEventArgs e)
        {
            _main.ParseLastprofFile();
        }

        private void Detect(object sender, RoutedEventArgs e)
        {
            _main.DetectLastprof();
        }
    }
}