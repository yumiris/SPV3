using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;

namespace Atarashii.GUI.Detector
{
    public class Main : INotifyPropertyChanged
    {
        private string _detectedPath;
        private string _logs;

        /// <summary>
        ///     Detected HCE executable path.
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
        ///     Invokes the HCE executable path detection.
        /// </summary>
        public void DetectExecutablePath()
        {
            try
            {
                DetectedPath = ExecutableFactory.Get(ExecutableFactory.Type.Detect).Path;
                AppendToLog("Executable found!");
            }
            catch (FileNotFoundException e)
            {
                AppendToLog(e.Message);
            }
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