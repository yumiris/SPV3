/**
 * Copyright (C) 2018-2019 Emilian Roman
 * 
 * This file is part of HCE.HCE.BalsamV.
 * 
 * HCE.HCE.BalsamV is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * HCE.HCE.BalsamV is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with HCE.HCE.BalsamV.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using HCE.BalsamV.Profile;
using HCE.BalsamV.Settings;
using Microsoft.Win32;

namespace HCE.BalsamV.GUI
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
            InitialiseComboBoxes();

            _main = (Main) DataContext;
            _main.Initialise();
        }

        /// <summary>
        ///     Assigns the Atarashii enumerators to the combo boxes.
        /// </summary>
        private void InitialiseComboBoxes()
        {
            ColourComboBox.ItemsSource = Enum.GetValues(typeof(Colour)).Cast<Colour>();
            ConnectionComboBox.ItemsSource = Enum.GetValues(typeof(Connection)).Cast<Connection>();
            FrameRateComboBox.ItemsSource = Enum.GetValues(typeof(FrameRate)).Cast<FrameRate>();
            TextureQualityComboBox.ItemsSource = Enum.GetValues(typeof(Quality)).Cast<Quality>();
            ParticlesComboBox.ItemsSource = Enum.GetValues(typeof(Particles)).Cast<Particles>();
            AudioQualityComboBox.ItemsSource = Enum.GetValues(typeof(Quality)).Cast<Quality>();
            VarietyComboBox.ItemsSource = Enum.GetValues(typeof(Quality)).Cast<Quality>();
        }

        /// <summary>
        ///     Invokes the blam.sav file picker.
        /// </summary>
        private void Load(object sender, RoutedEventArgs routedEventArgs)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Profile Binary|blam.sav"
            };

            if (dialog.ShowDialog() == true)
                _main.Path = dialog.FileName;
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            _main.Save();
        }

        private void About(object sender, RoutedEventArgs e)
        {
            Process.Start("https://github.com/yumiris/HCE.HCE.BalsamV");
        }

        private void Releases(object sender, RoutedEventArgs e)
        {
            Process.Start("https://github.com/yumiris/HCE.HCE.BalsamV/releases");
        }

        private void Version(object sender, RoutedEventArgs e)
        {
            Process.Start($"https://github.com/yumiris/HCE.HCE.BalsamV/releases/{_main.Version}");
        }
    }
}