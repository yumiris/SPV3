using System.Diagnostics;
using System.IO;
using System.Windows;

namespace SPV3.Settings.GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void HceSettings(object sender, RoutedEventArgs e)
        {
            if (File.Exists("BalsamV.GUI.exe")) Process.Start("BalsamV.GUI.exe");
        }

        private void Spv3Shaders(object sender, RoutedEventArgs e)
        {
            if (File.Exists("SPV3.Shaders.GUI.exe")) Process.Start("SPV3.Shaders.GUI.exe");
        }
    }
}