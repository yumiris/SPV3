using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Atarashii.GUI.Detector
{
    public class Main : INotifyPropertyChanged
    {
        private string _detectedPath;
        private string _logs;
        
        /// <summary>
        /// Detected HCE executable path.
        /// </summary>
        public string DetectedPath
        {
            get => _detectedPath;
            set
            {
                if (value == _detectedPath) return;
                _detectedPath = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Log messages to output to the GUI.
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

        /// <summary>
        ///     Invokes the HCE executable path detection.
        /// </summary>
        public void DetectExecutablePath()
        {
            DetectedPath = new Executable.Detector().Detect();
            AppendToLog(string.IsNullOrWhiteSpace(DetectedPath)
                ? "Legally installed executable not found."
                : "Executable found!");
        }

        /// <summary>
        /// Adds a given message to the log property.
        /// </summary>
        /// <param name="message">
        /// Message to append to the log.
        /// </param>
        private void AppendToLog(string message)
        {
            Logs = $"{message}\n\n{Logs}";
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}