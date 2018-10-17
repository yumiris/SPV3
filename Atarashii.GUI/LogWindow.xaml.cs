using System;
using System.IO;
using System.Windows;
using Microsoft.Win32;

namespace Atarashii.GUI
{
    /// <summary>
    ///     Interaction logic for LogWindow.xaml
    /// </summary>
    public partial class LogWindow : ILogger
    {
        public LogWindow()
        {
            InitializeComponent();
        }

        /// <inheritdoc />
        /// <summary>
        ///     Adds a given message to the log property.
        /// </summary>
        /// <param name="message">
        ///     Message to append to the log.
        /// </param>
        public void Log(string message)
        {
            var output = $"{DateTime.Now:s}: {message}";

            OutputTextBox.Text = string.IsNullOrWhiteSpace(OutputTextBox.Text)
                ? $"{output}"
                : $"{OutputTextBox.Text}\n\n{output}";

            OutputTextBox.ScrollToEnd();
        }

        private void Copy(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(OutputTextBox.Text))
            {
                Clipboard.SetText(OutputTextBox.Text);
                Log("Copied log to the clipboard.");
            }
            else
            {
                Log("Refusing to copy empty log to the clipboard.");
            }
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(OutputTextBox.Text))
            {
                Log("Refusing to save empty log.");
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
            Log($"Saved log to {dlg.FileName}");
        }

        private void Clear(object sender, RoutedEventArgs e)
        {
            OutputTextBox.Clear();
        }
    }
}