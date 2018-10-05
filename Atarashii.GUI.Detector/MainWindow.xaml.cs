using System.Windows;

namespace Atarashii.GUI.Detector
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

        private void Detect(object sender, RoutedEventArgs e) => _main.DetectExecutablePath();
    }
}