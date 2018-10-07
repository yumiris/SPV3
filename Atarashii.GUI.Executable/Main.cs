using System.IO;
using System.Windows;
using Atarashii.Exceptions;

namespace Atarashii.GUI.Executable
{
    /// <summary>
    ///     HCE Atarashii GUI main entity
    /// </summary>
    public class Main : BaseModel
    {
        private string _hcePath;

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
                    LogWindow.Output("Cleared selection.");
                else
                    LogWindow.Output($"Selected {value}.");
            }
        }

        /// <summary>
        ///     Invokes the HCE executable path detection.
        /// </summary>
        public void DetectExecutablePath()
        {
            try
            {
                HcePath = ExecutableFactory.Get(ExecutableFactory.Type.Detect).Path;
                LogWindow.Output($"Executable found: {HcePath}");
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
            if (!string.IsNullOrWhiteSpace(HcePath))
            {
                Clipboard.SetText(HcePath);
                LogWindow.Output("Copied HCE executable path to the clipboard.");
            }
            else
            {
                LogWindow.Output("Refusing to copy empty path to the clipboard.");
            }
        }

        /// <summary>
        ///     Attempt to load the HCE executable.
        /// </summary>
        public void Load()
        {
            try
            {
                new Atarashii.Executable(HcePath).Load();
                LogWindow.Output($"Successfully loaded {HcePath}");
            }
            catch (LoaderException e)
            {
                LogWindow.Output(e.Message);
            }
        }
    }
}