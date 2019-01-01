/**
 * Copyright (C) 2019 Emilian Roman
 * 
 * This file is part of SPV3.Installer.
 * 
 * SPV3.Installer is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * SPV3.Installer is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with SPV3.Installer.  If not, see <http://www.gnu.org/licenses/>.
 */

using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

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

        /// <summary>
        ///     Invokes directory picker.
        /// </summary>
        private void BrowseSource(object sender, RoutedEventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.ShowDialog();
                _main.Source = dialog.SelectedPath;
            }
        }

        /// <summary>
        ///     Invokes directory picker.
        /// </summary>
        private void BrowseTarget(object sender, RoutedEventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.ShowDialog();
                _main.Target = dialog.SelectedPath;
            }
        }

        /// <summary>
        ///     Invokes the Main Compile method.
        /// </summary>
        private async void Compile(object sender, RoutedEventArgs e)
        {
            CompileButton.Content = "Creating...";
            CompileButton.IsEnabled = false;

            using (var timer = new Timer())
            {
                timer.Tick += (s, e2) =>
                {
                    switch (CompileButton.Content)
                    {
                        case "":
                            CompileButton.Content = ".";
                            break;
                        case ".":
                            CompileButton.Content = "..";
                            break;
                        case "..":
                            CompileButton.Content = "";
                            break;
                        default:
                            CompileButton.Content = "";
                            break;
                    }
                };

                timer.Interval = 100;
                timer.Enabled = true;

                await Task.Run(() => { _main.Compile(); });
            }

            CompileButton.Content = "Compile Installer";
            CompileButton.IsEnabled = true;
        }

        /// <summary>
        ///     Invokes the GitHub repository page.
        /// </summary>
        private void About(object sender, RoutedEventArgs e)
        {
            Process.Start("https://github.com/yumiris/SPV3.Installer");
        }

        /// <summary>
        ///     Invokes the GitHub releases page.
        /// </summary>
        private void Releases(object sender, RoutedEventArgs e)
        {
            Process.Start("https://github.com/yumiris/SPV3.Installer/releases");
        }
    }
}
