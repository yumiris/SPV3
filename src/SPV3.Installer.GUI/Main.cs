using System;
using System.ComponentModel;
using System.IO;
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
        private const string LogFile = "SPV3.Installer.log";

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

        /// <summary>
        ///     Asynchronously resolves the default manifest, and invokes the Installer's Install method.
        ///     Any caught exceptions will be logged to %APPDATA%\SPV3.Installer.log
        /// </summary>
        public async void Install()
        {
            await Task.Run(() =>
            {
                try
                {
                    var manifest = ManifestRepository.LoadDefault();
                    new Installer(manifest, (Directory) Target, this).Install();
                }
                catch (Exception exception)
                {
                    var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                    var logPath = Path.Combine(appData, LogFile);

                    File.WriteAllText(logPath, exception.ToString());
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
            Status = $"{data}\n{Status}";
        }
    }
}