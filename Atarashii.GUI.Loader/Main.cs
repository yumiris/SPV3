using Atarashii.Exceptions;

namespace Atarashii.GUI.Loader
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
        ///     Attempt to load the HCE executable.
        /// </summary>
        public void Load()
        {
            try
            {
                new Executable(HcePath).Load();
                LogWindow.Output($"Successfully loaded {HcePath}");
            }
            catch (LoaderException e)
            {
                LogWindow.Output(e.Message);
            }
        }
    }
}