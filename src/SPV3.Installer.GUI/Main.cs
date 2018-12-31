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