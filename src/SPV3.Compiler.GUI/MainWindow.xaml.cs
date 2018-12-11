using System;
using System.IO;
using System.Windows;
using Microsoft.Win32;

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

        private void CreatePackage(object sender, RoutedEventArgs e)
        {
            _main.SavePackage();
        }

        private void CreateFile(object sender, RoutedEventArgs e)
        {
            _main.SaveFile();
        }

        private void DeleteFile(object sender, RoutedEventArgs e)
        {
            _main.DeleteFile();
        }

        private void DeletePackage(object sender, RoutedEventArgs e)
        {
            _main.DeletePackage();
        }

        private void BatchFiles(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Multiselect = true,
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };

            if (dialog.ShowDialog() != true) return;

            foreach (var filename in dialog.FileNames)
            {
                _main.SelectedPackage.Files.Add(new Main.File
                {
                    Name = Path.GetFileName(filename),
                    Description = $"Data for {Path.GetFileName(filename)}"
                });
            }
        }

        private void CreateInstaller(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog
            {
                FileName = "metadata.bin",
                Filter = "Installer Metadata|metadata.bin"
            };

            if(dialog.ShowDialog() == true)
                _main.SaveTo(dialog.FileName);
        }
    }
}