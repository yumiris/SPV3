/**
 * Copyright (c) 2018-2019 Emilian Roman
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

using System;
using System.IO;
using System.Windows;
using HXE;
using Microsoft.Win32;

namespace Eaxir
{
  /// <summary>
  ///   Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    private readonly Main _main;

    public MainWindow()
    {
      InitializeComponent();
      _main = (Main) DataContext;

      try
      {
        _main.LoadHceGamePath();
      }
      catch (Exception e)
      {
        Output(e.Message + " -- You'll need to manually select the HCE path.");
      }

      try
      {
        _main.LoadProfile();
      }
      catch (Exception e)
      {
        Output(e.Message + " -- You'll need to manually select the profile path.");
      }
    }

    private void Save(object sender, RoutedEventArgs e)
    {
      try
      {
        _main.Save();
        Output("Successfully installed DSOAL and enabled EAX!");
      }
      catch (Exception exception)
      {
        Output(exception.Message);
      }
    }

    private void Output(string message)
    {
      ConsoleTextBox.Text = message + "\n\n" + ConsoleTextBox.Text;
    }

    private void BrowseProfilePath(object sender, RoutedEventArgs e)
    {
      var dialog = new OpenFileDialog
      {
        Filter = "HCE profiles (blam.sav)|blam.sav"
      };

      if (dialog.ShowDialog() != true) return;

      _main.Profile = (Profile) dialog.FileName;
      _main.Profile.Load();

      _main.ProfilePath = dialog.FileName;
      _main.ProfileName = _main.Profile.Details.Name;
    }

    private void BrowseHceGamePath(object sender, RoutedEventArgs e)
    {
      var dialog = new OpenFileDialog
      {
        Filter = "HCE executable (haloce.exe)|haloce.exe"
      };

      if (dialog.ShowDialog() == true)
        _main.HceGamePath = Path.GetDirectoryName(dialog.FileName);
    }
  }
}