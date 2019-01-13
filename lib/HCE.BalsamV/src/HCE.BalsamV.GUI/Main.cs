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
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using HCE.BalsamV.GUI.Properties;

namespace HCE.BalsamV.GUI
{
    /// <summary>
    ///     Main model for the HCE.HCE.BalsamV program.
    /// </summary>
    public sealed class Main : INotifyPropertyChanged
    {
        /// <summary>
        ///     Atarashii Profile Configuration for the selected blam.sav.
        /// </summary>
        private Blam _blam;

        /// <summary>
        ///     Selected blam.sav exists.
        /// </summary>
        private bool _canEdit;

        /// <summary>
        ///     Selected Blam.sav absolute path.
        /// </summary>
        private string _path;

        /// <summary>
        ///     <see cref="_path" />
        /// </summary>
        public string Path
        {
            get => _path;
            set
            {
                if (value == _path) return;
                _path = value;
                OnPropertyChanged();
                OnPathChanged();
            }
        }

        /// <summary>
        ///     <see cref="_blam" />
        /// </summary>
        public Blam Blam
        {
            get => _blam;
            set
            {
                if (Equals(value, _blam)) return;
                _blam = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     <see cref="_canEdit" />
        /// </summary>
        public bool CanEdit
        {
            get => _canEdit;
            set
            {
                if (value == _canEdit) return;
                _canEdit = value;
                OnPropertyChanged();
            }
        }

        public string Version
        {
            get
            {
                using (var stream = Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream("HCE.HCE.BalsamV.GUI.Resources.Version.txt"))
                using (var reader = new StreamReader(stream))
                    return reader.ReadToEnd().Trim();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Attempts to auto-detect & load a blam.sav on the file system.
        ///     Initialisation is done with code directly taken from <see cref="BlamFactory.GetFromSystem" />,
        ///     for the purpose of exposing the deduced absolute path of the blam.sav binary.
        /// </summary>
        public void Initialise()
        {
            try
            {
                var name = LastprofFactory.DetectOnSystem().Name;
                Path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                    "My Games", "Halo CE", "savegames", name, "blam.sav");

                Blam = BlamFactory.GetFromBinary(Path);
                CanEdit = true;
            }
            catch (Exception)
            {
                MessageBox.Show("Could not detect blam.sav. Please load manually!");
                CanEdit = false;
            }
        }

        /// <summary>
        ///     Invoke patching of the blam.sav with the current Blam state.
        /// </summary>
        public void Save()
        {
            try
            {
                new BlamPatcher(Blam).PatchToBinary(Path);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        /// <summary>
        ///     Invoke parsing of the selected blam.sav upon path change.
        /// </summary>
        private void OnPathChanged()
        {
            try
            {
                Blam = BlamFactory.GetFromBinary(Path);
                CanEdit = true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                CanEdit = false;
            }
        }

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}