using System;
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
            if (!string.IsNullOrWhiteSpace(OutputTextBox.Text))
            {
                Clipboard.SetText(OutputTextBox.Text);
                Output("Copied log to the clipboard.");
            }
            else
            {
                Output("Refusing to copy empty log to the clipboard.");
            }
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(OutputTextBox.Text))
            {
                Output("Refusing to save empty log.");
                return;
            }
            
            var dlg = new SaveFileDialog
            {
                FileName = "atarashii",
                DefaultExt = ".log",
                Filter = "Atarashii Log (.log)|*.log"
            };

            var result = dlg.ShowDialog();

            if (result != true) return;

            File.WriteAllText(dlg.FileName, OutputTextBox.Text);
            Output($"Saved log to {dlg.FileName}");
        }

        /// <summary>
        ///     Adds a given message to the log property.
        /// </summary>
        /// <param name="message">
        ///     Message to append to the log.
        /// </param>
        public void Output(string message)
        {
            var output = $"{DateTime.Now:s}: {message}";

            OutputTextBox.Text = string.IsNullOrWhiteSpace(OutputTextBox.Text)
                ? $"{output}"
                : $"{OutputTextBox.Text}\n\n{output}";

            OutputTextBox.ScrollToEnd();
        }

        private void Clear(object sender, RoutedEventArgs e)
        {
            OutputTextBox.Clear();
        }
    }
}