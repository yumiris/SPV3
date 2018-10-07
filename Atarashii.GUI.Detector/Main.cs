using System.IO;

namespace Atarashii.GUI.Detector
{
    public class Main : BaseModel
    {
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
    }
}