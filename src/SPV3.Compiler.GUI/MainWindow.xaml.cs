using System;
using System.Diagnostics;
using System.Threading.Tasks;
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

        private async void Compile(object sender, RoutedEventArgs e)
        {
            try
            {
                AppendOutput("Invoked installation creation...");
                AppendOutput("Please wait!");

                CompileButton.Content = "Creating...";
                CompileButton.IsEnabled = false;
                
                using (var timer = new System.Windows.Forms.Timer())
                {
                    timer.Tick += (s, e2) =>
                    {
                        switch (CompileButton.Content)
                        {
                            case "":
                                CompileButton.Content = ".";
                                break;
                            case ".":
                                CompileButton.Content = "..";
                                break;
                            case "..":
                                CompileButton.Content = "";
                                break;
                            default:
                                CompileButton.Content = "";
                                break;
                        }
                    };

                    timer.Interval = 100;
                    timer.Enabled = true;

                    await Task.Run(() =>
                    {
                        _main.Compile();
                    });
                }
                
                CompileButton.Content = "Compile Installer";
                CompileButton.IsEnabled = true;

                AppendOutput("Successfully compiled source data!");
            }
            catch (Exception exception)
            {
                AppendOutput(exception.Message);
                AppendOutput(exception.StackTrace);
            }
        }

        private void AppendOutput(string text)
        {
            OutputText.Text += $"{text}\n";
            OutputText.ScrollToEnd();
        }

        private void About(object sender, RoutedEventArgs e)
        {
            Process.Start("https://github.com/yumiris/SPV3.Installer");
        }

        private void Releases(object sender, RoutedEventArgs e)
        {
            Process.Start("https://github.com/yumiris/SPV3.Installer/releases");
        }
    }
}