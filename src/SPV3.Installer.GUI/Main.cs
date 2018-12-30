using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using SPV3.Domain;
using SPV3.Installer.GUI.Annotations;
using Directory = SPV3.Domain.Directory;
using File = System.IO.File;

namespace SPV3.Installer.GUI
{
    public class Main : INotifyPropertyChanged, IStatus
    {
        private string _target;
        private string _status = "Awaiting end-user invocation...";

        public string Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged();
            }
        }

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

        public async void Install()
        {
            await Task.Run(() =>
            {
                CommitStatus("Resolving default manifest...");
                var manifest = ManifestRepository.LoadDefault();
                var installer = new Installer(manifest, (Directory) Target, this);

                try
                {
                    installer.Install();
                }
                catch (Exception exception)
                {
                    File.WriteAllText("SPV3.Installer.log", exception.ToString());
                }
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void CommitStatus(string data)
        {
            Status = data;
        }
    }
}