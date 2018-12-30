using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace SPV3.Installer.GUI
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
        }

        private async void Install(object sender, RoutedEventArgs e)
        {
            InstallButton.Content = "Installing...";
            InstallButton.IsEnabled = false;

            using (var timer = new Timer())
            {
                timer.Tick += (s, e2) =>
                {
                    switch (InstallButton.Content)
                    {
                        case "":
                            InstallButton.Content = ".";
                            break;
                        case ".":
                            InstallButton.Content = "..";
                            break;
                        case "..":
                            InstallButton.Content = "";
                            break;
                        default:
                            InstallButton.Content = "";
                            break;
                    }
                };

                timer.Interval = 100;
                timer.Enabled = true;

                await Task.Run(() => { _main.Install(); });
            }

            InstallButton.Content = "Install SPV3";
            InstallButton.IsEnabled = true;
        }

        /// <summary>
        ///     Invokes directory picker.
        /// </summary>
        private void Browse(object sender, RoutedEventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.ShowDialog();
                _main.Target = dialog.SelectedPath;
            }
        }

        /// <summary>
        ///     Invokes the GitHub repository page.
        /// </summary>
        private void About(object sender, RoutedEventArgs e)
        {
            Process.Start("https://github.com/yumiris/SPV3.Installer");
        }

        /// <summary>
        ///     Invokes the GitHub releases page.
        /// </summary>
        private void Releases(object sender, RoutedEventArgs e)
        {
            Process.Start("https://github.com/yumiris/SPV3.Installer/releases");
        }
    }
}