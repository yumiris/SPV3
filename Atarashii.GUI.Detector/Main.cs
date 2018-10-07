using System.IO;
using System.Windows;

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
                LogWindow.Output($"Executable found: {DetectedPath}");
            }
            catch (FileNotFoundException e)
            {
                LogWindow.Output(e.Message);
            }
        }

        /// <summary>
        ///     Copies the detected HCE path to the clipboard.
        /// </summary>
        public void CopyToClipboard()
        {
            if (!string.IsNullOrWhiteSpace(DetectedPath))
            {
                Clipboard.SetText(DetectedPath);
                LogWindow.Output("Copied detected HCE path to the clipboard.");
            }
            else
            {
                LogWindow.Output("Refusing to copy null result to the clipboard.");
            }
        }
    }
}