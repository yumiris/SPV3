using System;
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

        private async void Create(object sender, RoutedEventArgs e)
        {
            try
            {
                AppendOutput("Invoked installation creation...");
                AppendOutput("Please wait!");

                CreateButton.Content = "Creating...";
                CreateButton.IsEnabled = false;
                
                using (var timer = new System.Windows.Forms.Timer())
                {
                    timer.Tick += (s, e2) =>
                    {
                        switch (CreateButton.Content)
                        {
                            case "":
                                CreateButton.Content = ".";
                                break;
                            case ".":
                                CreateButton.Content = "..";
                                break;
                            case "..":
                                CreateButton.Content = "";
                                break;
                            default:
                                CreateButton.Content = "";
                                break;
                        }
                    };

                    timer.Interval = 100;
                    timer.Enabled = true;

                    await Task.Run(() =>
                    {
                        _main.Create();
                    });
                }
                
                CreateButton.Content = "Create Installer";
                CreateButton.IsEnabled = true;

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
    }
}