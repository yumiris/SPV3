using System.IO;
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
                    LogWindow.Log("Cleared selection.");
                else
                    LogWindow.Log($"Selected {value}.");
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
                LogWindow.Log($"Executable found: {HcePath}");
            }
            catch (FileNotFoundException e)
            {
                LogWindow.Log(e.Message);
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
                LogWindow.Log($"Successfully loaded {HcePath}");
            }
            catch (LoaderException e)
            {
                LogWindow.Log(e.Message);
            }
        }
    }
}