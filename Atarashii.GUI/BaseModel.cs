using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Atarashii.GUI.Properties;
using Microsoft.Win32;

namespace Atarashii.GUI
{
    public class BaseModel : INotifyPropertyChanged
    {
        public LogWindow LogWindow { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Shows the log window to the right of the given window.
        /// </summary>
        /// <param name="window">
        ///     Window to calculate the dimensions of.
        /// </param>
        public void ShowLogWindow(Window window)
        {
            LogWindow = new LogWindow();
            LogWindow.Show();
            LogWindow.Left += window.Width * 1.5;
        }

        /// <summary>
        ///     Opens up a file picking dialogue window.
        /// </summary>
        /// <param name="filter">
        ///     File filter to use in the dialogue window.
        /// </param>
        /// <returns>
        ///     File name chosen by the end-user.
        /// </returns>
        public string PickFile(string filter = null)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = filter ?? string.Empty
            };

            return openFileDialog.ShowDialog() == true
                ? openFileDialog.FileName
                : string.Empty;
        }

        /// <summary>
        ///     Copies given data to the clipboard.
        /// </summary>
        public void CopyToClipboard(string data)
        {
            if (!string.IsNullOrWhiteSpace(data))
            {
                Clipboard.SetText(data);
                LogWindow.Log("Copied data to the clipboard.");
            }
            else
            {
                LogWindow.Log("Refusing to copy empty data to the clipboard.");
            }
        }

        /// <summary>
        ///     Exits the application with exit code 0.
        /// </summary>
        public void Exit()
        {
            Environment.Exit(0);
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}