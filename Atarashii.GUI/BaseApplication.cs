using System;
using System.Windows;
using Microsoft.Win32;

namespace Atarashii.GUI
{
    public static class BaseApplication
    {
        public static void Initialise(Window window, BaseModel model)
        {
            model.LogWindow = new LogWindow();
            model.LogWindow.Show();
            model.LogWindow.Left += window.Width * 1.5;
        }

        public static string PickFile(string filter)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = filter
            };

            return openFileDialog.ShowDialog() == true
                ? openFileDialog.FileName
                : string.Empty;
        }

        public static void Exit()
        {
            Environment.Exit(0);
        }
    }
}