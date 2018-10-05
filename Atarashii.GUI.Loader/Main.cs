using System.ComponentModel;
using System.Runtime.CompilerServices;
using Atarashii.Executable;

namespace Atarashii.GUI.Loader
{
    /// <summary>
    ///     HCE Atarashii GUI main entity
    /// </summary>
    public class Main : INotifyPropertyChanged
    {
        private readonly Executable.Loader _loader = new Executable.Loader();
        private readonly Verifier _verifier = new Verifier();

        private string _hcePath;
        private string _logs;

        /// <summary>
        ///     HCE executable path.
        /// </summary>
        public string HcePath
        {
            get => _hcePath;
            set
            {
                if (value == _hcePath) return;
                _hcePath = value;
                OnPropertyChanged();

                if (string.IsNullOrWhiteSpace(value))
                    AppendToLog("Cleared selection.");
                else
                    AppendToLog($"Selected {value}.");
            }
        }

        /// <summary>
        ///     Log messages to output to the GUI.
        /// </summary>
        public string Logs
        {
            get => _logs;
            set
            {
                if (value == _logs) return;
                _logs = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Attempt to load the HCE executable.
        /// </summary>
        public void Load()
        {
            try
            {
                _loader.Execute(HcePath, _verifier);
                AppendToLog($"Successfully loaded {HcePath}");
            }
            catch (LoaderException e)
            {
                AppendToLog(e.Message);
            }
        }

        /// <summary>
        ///     Invokes HCE executable detection.
        /// </summary>
        public void AttemptDetection()
        {
            HcePath = new Detector().Detect();
        }

        /// <summary>
        ///     Adds a given message to the log property.
        /// </summary>
        /// <param name="message">
        ///     Message to append to the log.
        /// </param>
        private void AppendToLog(string message)
        {
            Logs = $"{message}\n\n{Logs}";
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}