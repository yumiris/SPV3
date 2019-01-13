/**
 * Copyright (C) 2019 Emilian Roman
 * 
 * This file is part of SPV3.Settings.
 * 
 * SPV3.Settings is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * SPV3.Settings is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with SPV3.Settings.  If not, see <http://www.gnu.org/licenses/>.
 */

﻿using System;
using System.Linq;
using System.Windows;
using HCE.BalsamV.Settings;

namespace SPV3.Settings.GUI
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
            FrameRateComboBox.ItemsSource = Enum.GetValues(typeof(FrameRate)).Cast<FrameRate>();
            TextureQualityComboBox.ItemsSource = Enum.GetValues(typeof(Quality)).Cast<Quality>();
            ParticlesComboBox.ItemsSource = Enum.GetValues(typeof(Particles)).Cast<Particles>();
            AudioQualityComboBox.ItemsSource = Enum.GetValues(typeof(Quality)).Cast<Quality>();
            VarietyComboBox.ItemsSource = Enum.GetValues(typeof(Quality)).Cast<Quality>();
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            _main.Save();
        }

        private void Spv3Shaders(object sender, RoutedEventArgs e)
        {
            _main.Shaders();
        }
    }
}