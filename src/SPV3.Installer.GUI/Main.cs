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
        ///     Asynchronously resolves the default manifest, and invokes the Installer's Install method.
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