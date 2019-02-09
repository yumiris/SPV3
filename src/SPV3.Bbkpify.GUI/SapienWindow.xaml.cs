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

using System.Windows;
using Microsoft.Win32;

namespace SPV3.Bbkpify.GUI
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
            Close();
        }

        private void PickPath(object sender, RoutedEventArgs e)
        {
            var placeholderDialog = new OpenFileDialog
            {
                Filter = "Executables (*.exe)|*.exe|All files (*.*)|*.*"
            };

            if (placeholderDialog.ShowDialog() == true) main.SapienExecutable = placeholderDialog.FileName;
        }
    }
}