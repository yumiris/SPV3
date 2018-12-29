using System.ComponentModel;
using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using SPV3.Installer.Data;
using SPV3.Installer.GUI.Properties;
using Directory = SPV3.Domain.Directory;

namespace SPV3.Installer.GUI
{
    public class Main : INotifyPropertyChanged
    {
        private string _target; 
        private string _status;

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
                var manifest = new ManifestRepository((Domain.File) "0x00.bin").Load();
                var target = (Directory) Target;

                foreach (var package in manifest.Packages)
                {
                    Status = $"Installing {(string) package.Name} to {(string) target}...";
                    ZipFile.ExtractToDirectory(package.Name, target);
                }
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}