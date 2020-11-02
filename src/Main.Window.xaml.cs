/**
 * Copyright (c) 2019 Emilian Roman
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
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace SPV3
{
  /// <summary>
  ///   Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class Main_Window
  {
    private readonly Main _main;

    public Main_Window()
    {
      InitializeComponent();
      MainWindow.WindowTitleBrush = new SolidColorBrush(Color.FromArgb(100, 21, 43, 82));

      _main = (Main) DataContext;
      _main.Initialise();

      ConfigurationUserControl.Home += Main;
      ReportUserControl.Home        += Main;
      InstallUserControl.Home       += Main;
      CompileUserControl.Home       += Main;
      VersionUserControl.Update     += Update;
    }

    private async void Load(object sender, RoutedEventArgs e)
    {
      LoadButton.IsEnabled = false;
      await Task.Run(() => { _main.Invoke(); });
      LoadButton.IsEnabled = true;
    }

    private async void Assets(object sender, RoutedEventArgs e)
    {
      AssetsButton.IsEnabled = false;
      await Task.Run(() => { _main.Update(); });
      AssetsButton.IsEnabled = true;
    }

    private void Quit(object sender, RoutedEventArgs e)
    {
      _main.Quit();
    }

    private void Report(object sender, MouseButtonEventArgs e)
    {
      MainTabControl.SelectedItem = ReportTabItem;
      ReportUserControl.Report.Initialise();
    }

    private void Main(object sender, EventArgs e)
    {
      MainTabControl.SelectedItem = MainTabItem;
    }

    private void Update(object sender, EventArgs e)
    {
      MainTabControl.SelectedItem = UpdateTabItem;
    }

    private void Install(object sender, RoutedEventArgs e)
    {
      MainTabControl.SelectedItem = InstallTabItem;
    }

    private void Compile(object sender, RoutedEventArgs e)
    {
      MainTabControl.SelectedItem = CompileTabItem;
    }

    private void Settings(object sender, RoutedEventArgs e)
    {
      MainTabControl.SelectedItem = ConfigurationTabItem;
    }
  }
}