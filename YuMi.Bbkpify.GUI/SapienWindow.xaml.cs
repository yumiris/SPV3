using System;
using System.Windows;
using Microsoft.Win32;

namespace YuMi.Bbkpify.GUI
{
    public partial class SapienWindow : Window
    {
        private readonly Sapien sapien = new Sapien();
        
        public SapienWindow()
        {
            DataContext = sapien;
            InitializeComponent();
            sapien.LoadPath();
        }

        private void SavePath(object sender, RoutedEventArgs e)
        {
            sapien.SavePath();
            System.Windows.Forms.Application.Restart();
            Environment.Exit(0);
        }

        private void PickPath(object sender, RoutedEventArgs e)
        {
            var placeholderDialog = new OpenFileDialog
            {
                Filter = "Executables (*.exe)|*.exe|All files (*.*)|*.*"
            };
            
            if (placeholderDialog.ShowDialog() == true)
            {
                sapien.Path = placeholderDialog.FileName;
            }
        }
    }
}
