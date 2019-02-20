/**
 * Copyright (C) 2018-2019 Emilian Roman
 * 
 * This file is part of SPV3.Bbkpify.
 * 
 * SPV3.Bbkpify is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * SPV3.Bbkpify is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with SPV3.Bbkpify.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Windows;
using Microsoft.Win32;

namespace SPV3.Bbkpify.GUI
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    private readonly Main main;

    public MainWindow()
    {
      InitializeComponent();
      main = (Main) DataContext;
      main.LoadConfig();
      main.SaveConfig();
    }

    private void Commit(object sender, RoutedEventArgs e) => main.Commit();
    private void Revert(object sender, RoutedEventArgs e) => main.Revert();

    private void LoadSapien(object    sender, RoutedEventArgs e) => main.LoadSapien();
    private void LoadBbkpify(object   sender, RoutedEventArgs e) => main.LoadBbkpify();
    private void LoadUnbbkpify(object sender, RoutedEventArgs e) => main.LoadUnbbkpify();

    private void ConfigSapien(object sender, RoutedEventArgs e) => new SapienWindow(main).Show();
    private void Quit(object         sender, RoutedEventArgs e) => Environment.Exit(0);

    private void ChoosePlaceholder(object sender, RoutedEventArgs e)
    {
      var placeholderDialog = new OpenFileDialog
      {
        Filter = "Bitmap files (*.bitmap)|*.bitmap|All files (*.*)|*.*"
      };

      if (placeholderDialog.ShowDialog() == true)
      {
        main.Placeholder = placeholderDialog.FileName;
      }
    }

    private void ChooseDirectory(object sender, RoutedEventArgs e)
    {
      using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
      {
        dialog.ShowDialog();
        main.Directory = dialog.SelectedPath;
      }
    }
  }
}