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

using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using SPV3.Domain;
using SPV3.Installer.GUI.Annotations;
using SPV3.Installer.Installers;
using Directory = SPV3.Domain.Directory;

namespace SPV3.Installer.GUI
{
    /// <summary>
    ///     Main model used for the UI bindings.
    /// </summary>
    public class Main : INotifyPropertyChanged, IStatus
    {
        /// <summary>
        ///     <see cref="CanInstall" />
        /// </summary>
        private bool _canInstall;

        /// <summary>
        ///     <see cref="Status" />
        /// </summary>
        private string _status = "Awaiting end-user invocation...";

        /// <summary>
        ///     <see cref="Target" />
        /// </summary>
        private string _target;

        /// <summary>
        ///     Target directory path.
        /// </summary>
        public string Target
        {
            get => _target;
            set
            {
                if (value == _target) return;
                _target = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Status output.
        /// </summary>
        public string Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Allow installation.
        /// </summary>
        public bool CanInstall
        {
            get => _canInstall;
            set
            {
                if (value == _canInstall) return;
                _canInstall = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void CommitStatus(string data)
        {
            Status = $"{data}\n{Status}";
        }

        public void CommitStatus(string data, StatusType type)
        {
            var symbol = string.Empty;
            
            switch (type)
            {
                case StatusType.Success:
                    symbol = "✔";
                    break;
                case StatusType.Warning:
                    symbol = "⚠";
                    break;
                case StatusType.Failure:
                    symbol = "⯃";
                    break;
                case StatusType.Require:
                    symbol = "⌘";
                    break;
            }

            CommitStatus($"{symbol} | {data}");
        }

        /// <summary>
        ///     Resolves the default manifest, and invokes the Installer's Install method.
        /// </summary>
        public void Install()
        {
            try
            {
                Status = string.Empty;

                var target = (Directory) Target;
                var backup = (Directory) Path.Combine(Target, "SPV3-" + Guid.NewGuid());

                var manifest = ManifestRepository.LoadDefault();

                new MetaInstaller(target, backup, this).Install(manifest);
            }
            catch (Exception exception)
            {
                CommitStatus(exception.ToString());
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            NotifyCanInstall();
        }

        /// <summary>
        ///     Updates CanInstall. If Target directory exist on the filesystem, CanInstall becomes true.
        /// </summary>
        private void NotifyCanInstall()
        {
            CanInstall = System.IO.Directory.Exists(Target);
        }
    }
}
