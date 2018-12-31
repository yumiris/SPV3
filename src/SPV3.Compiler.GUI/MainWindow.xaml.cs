/**
 * Copyright (c) 2018 Emilian Roman
 * 
 * This software is provided 'as-is', without any express or implied
 * warranty. In no event will the authors be held liable for any damages
 * arising from the use of this software.
 * 
 * Permission is granted to anyone to use this software for any purpose,
 * including commercial applications, and to alter it and redistribute it
 * freely, subject to the following restrictions:
 * 
 * 1. The origin of this software must not be misrepresented; you must not
 *    claim that you wrote the original software. If you use this software
 *    in a product, an acknowledgment in the product documentation would be
 *    appreciated but is not required.
 * 2. Altered source versions must be plainly marked as such, and must not be
 *    misrepresented as being the original software.
 * 3. This notice may not be removed or altered from any source distribution.
 */

﻿using System.Diagnostics;
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