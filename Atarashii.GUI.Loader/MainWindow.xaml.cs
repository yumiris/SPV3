using System;
using System.ComponentModel;
using System.Windows;
using Microsoft.Win32;

namespace Atarashii.GUI.Loader
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
            _main.LogWindow = new LogWindow();
            _main.LogWindow.Show();
            _main.LogWindow.Left += Width * 1.5;
        }

        private void Load(object sender, RoutedEventArgs e)
        {
            _main.Load();
        }

        private void Browse(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "HCE Executable|haloce.exe"
            };

            if (openFileDialog.ShowDialog() == true) _main.HcePath = openFileDialog.FileName;
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}