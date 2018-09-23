using System;
using System.Windows;
using Microsoft.Win32;

namespace YuMi.Bbkpify.GUI
{
    public partial class SapienWindow : Window
    {
        private readonly Main main;
        
        public SapienWindow(Main main)
        {
            this.main = main;
            DataContext = main;
            InitializeComponent();
        }

        private void SavePath(object sender, RoutedEventArgs e)
        {
            main.SaveConfig();
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
                main.SapienExecutable = placeholderDialog.FileName;
            }
        }
    }
}
