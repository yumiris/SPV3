using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;

namespace Atarashii.GUI.Detector
{
    public class Main : INotifyPropertyChanged
    {
        public LogWindow LogWindow { get; set; }
        
        private string _detectedPath;
        
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

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Invokes the HCE executable path detection.
        /// </summary>
        public void DetectExecutablePath()
        {
            try
            {
                DetectedPath = ExecutableFactory.Get(ExecutableFactory.Type.Detect).Path;
                LogWindow.Output("Executable found!");
            }
            catch (FileNotFoundException e)
            {
                LogWindow.Output(e.Message);
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}