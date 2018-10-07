using System.IO;
using System.Windows;
using Microsoft.Win32;

namespace Atarashii.GUI
{
    /// <summary>
    ///     Interaction logic for LogWindow.xaml
    /// </summary>
    public partial class LogWindow : Window
    {
        public LogWindow()
        {
            InitializeComponent();
        }

        private void Copy(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(OutputTextBox.Text);
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            var dlg = new SaveFileDialog
            {
                FileName = "atarashii",
                DefaultExt = ".log",
                Filter = "Atarashii Log (.log)|*.log"
            };

            var result = dlg.ShowDialog();

            if (result == true) File.WriteAllText(dlg.FileName, OutputTextBox.Text);
        }
    }
}