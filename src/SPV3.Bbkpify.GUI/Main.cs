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

﻿using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace SPV3.Bbkpify.GUI
{
    public class Main : INotifyPropertyChanged
    {
        private const string BbkpifyExecutable = "SPV3.Bbkpify.CLI.exe";
        private const string UnbbkpifyExecutable = "SPV3.Unbbkpify.CLI.exe";
        private bool canLoadSapien;
        private bool diffPattern;
        private string directory;
        private bool multiPattern;

        private bool nrmlPattern;

        private string placeholder;

        private bool readyToCommit;
        private bool readyToRevert;

        private string sapienExecutable;

        /// <summary>
        ///     Location of the configuration file used for persistent values.
        /// </summary>
        private static string ConfigFile =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SPV3.Bbkpify.cfg");

        /// <summary>
        ///     Bitmaps placeholder path.
        /// </summary>
        public string Placeholder
        {
            get => placeholder;
            set
            {
                if (value == placeholder) return;
                placeholder = value;
                NotifyPropertyChanged();
                ValidateProperties();
                SaveConfig();
            }
        }

        /// <summary>
        ///     Bitmaps directory path.
        /// </summary>
        public string Directory
        {
            get => directory;
            set
            {
                if (value == directory) return;
                directory = value;
                NotifyPropertyChanged();
                ValidateProperties();
                SaveConfig();
            }
        }

        /// <summary>
        ///     NRML bitmaps search pattern.
        /// </summary>
        public bool NrmlPattern
        {
            get => nrmlPattern;
            set
            {
                if (value == nrmlPattern) return;
                nrmlPattern = value;
                NotifyPropertyChanged();
                ValidateProperties();
                SaveConfig();
            }
        }

        /// <summary>
        ///     MULTI bitmaps search pattern.
        /// </summary>
        public bool MultiPattern
        {
            get => multiPattern;
            set
            {
                if (value == multiPattern) return;
                multiPattern = value;
                NotifyPropertyChanged();
                ValidateProperties();
                SaveConfig();
            }
        }

        /// <summary>
        ///     DIFF bitmaps search pattern.
        /// </summary>
        public bool DiffPattern
        {
            get => diffPattern;
            set
            {
                if (value == diffPattern) return;
                diffPattern = value;
                NotifyPropertyChanged();
                ValidateProperties();
                SaveConfig();
            }
        }

        /// <summary>
        ///     The requirements to apply the placeholder have been met.
        /// </summary>
        public bool ReadyToCommit
        {
            get => readyToCommit;
            set
            {
                if (value == readyToCommit) return;
                readyToCommit = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        ///     The requirements to revert the placeholder have been met.
        /// </summary>
        public bool ReadyToRevert
        {
            get => readyToRevert;
            set
            {
                if (value == readyToRevert) return;
                readyToRevert = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        ///     Path of the Sapien executable.
        /// </summary>
        public string SapienExecutable
        {
            get => sapienExecutable;
            set
            {
                if (value == sapienExecutable) return;
                sapienExecutable = value;
                NotifyPropertyChanged();
                CheckSapien();
            }
        }

        /// <summary>
        ///     Checks if the Sapien executable exists.
        /// </summary>
        public bool CanLoadSapien
        {
            get => canLoadSapien;
            set
            {
                if (value == canLoadSapien) return;
                canLoadSapien = value;
                NotifyPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Checks if the property values are valid, thereby enabling commands.
        /// </summary>
        private void ValidateProperties()
        {
            ReadyToCommit = PlaceholderValidator.GetStatus(Placeholder) == PlaceholderStatus.IsValid &&
                            DirectoryValidator.GetStatus(Directory) == DirectoryStatus.IsValid &&
                            (NrmlPattern || MultiPattern || DiffPattern);

            ReadyToRevert = DirectoryValidator.GetStatus(Directory) == DirectoryStatus.IsValid;
        }

        /// <summary>
        ///     Calls the Bbkpify CLI to apply the placeholder the target directory.
        /// </summary>
        public void Commit()
        {
            var args = $"{Placeholder} {Directory}";

            if (NrmlPattern) Process.Start(BbkpifyExecutable, $"{args} {Bbkpify.Main.Patterns[0]}");

            if (MultiPattern) Process.Start(BbkpifyExecutable, $"{args} {Bbkpify.Main.Patterns[1]}");

            if (DiffPattern) Process.Start(BbkpifyExecutable, $"{args} {Bbkpify.Main.Patterns[2]}");
        }

        /// <summary>
        ///     Calls the Unbbkpify CLI to apply the placeholder the target directory.
        /// </summary>
        public void Revert()
        {
            Process.Start(UnbbkpifyExecutable, $"{Directory}");
        }

        /// <summary>
        ///     Saves the current state to a persistent file.
        /// </summary>
        public void SaveConfig()
        {
            var config = new Func<string>(() =>
            {
                var s = new StringBuilder();
                s.Append($"{Placeholder}|");
                s.Append($"{Directory}|");
                s.Append($"{NrmlPattern}|");
                s.Append($"{MultiPattern}|");
                s.Append($"{DiffPattern}|");
                s.Append($"{SapienExecutable}");
                return s.ToString();
            })();

            File.WriteAllText(ConfigFile, config);
        }

        /// <summary>
        ///     Loads the saved state to the properties.
        /// </summary>
        public void LoadConfig()
        {
            if (!File.Exists(ConfigFile)) SaveConfig();

            var config = File.ReadAllText(ConfigFile).Split('|');

            Placeholder = config[0];
            Directory = config[1];
            NrmlPattern = config[2].Equals("True");
            MultiPattern = config[3].Equals("True");
            DiffPattern = config[4].Equals("True");
            SapienExecutable = config[5];
        }

        private void CheckSapien()
        {
            CanLoadSapien = File.Exists(SapienExecutable);
        }

        /// <summary>
        ///     Runs the Sapien process.
        /// </summary>
        public void LoadSapien()
        {
            if (CanLoadSapien) Process.Start(SapienExecutable);
        }

        /// <summary>
        ///     Runs the Bbkpify CLI process.
        /// </summary>
        public void LoadBbkpify()
        {
            Process.Start(BbkpifyExecutable);
        }

        /// <summary>
        ///     Runs the Unbbkpify CLI process.
        /// </summary>
        public void LoadUnbbkpify()
        {
            Process.Start(UnbbkpifyExecutable);
        }

        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}