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

        public void ShowLogWindow(Window window)
        {
            LogWindow = new LogWindow();
            LogWindow.Show();
            LogWindow.Left += window.Width * 1.5;
        }

        public string PickFile(string filter)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = filter
            };

            return openFileDialog.ShowDialog() == true
                ? openFileDialog.FileName
                : string.Empty;
        }

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