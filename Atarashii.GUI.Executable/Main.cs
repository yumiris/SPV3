using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using Atarashii.Exceptions;

namespace Atarashii.GUI.Executable
{
    /// <summary>
    ///     HCE Atarashii GUI main entity
    /// </summary>
    public class Main : INotifyPropertyChanged
    {
        private Atarashii.Executable _executable;

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
                _executable = new Atarashii.Executable(value);
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
                if (_executable == null) _executable = ExecutableFactory.Get(ExecutableFactory.Type.Detect);
                _executable.Load();
                AppendToLog($"Successfully loaded {HcePath}");
            }
            catch (Exception e)
            {
                if (e is LoaderException || e is FileNotFoundException)
                    AppendToLog(e.Message);
            }
        }

        /// <summary>
        ///     Invokes the HCE executable path detection.
        /// </summary>
        public void Detect()
        {
            try
            {
                HcePath = ExecutableFactory.Get(ExecutableFactory.Type.Detect).Path;
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