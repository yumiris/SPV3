using System.ComponentModel;
using System.Runtime.CompilerServices;
using Atarashii.Exceptions;

namespace Atarashii.GUI.Loader
{
    /// <summary>
    ///     HCE Atarashii GUI main entity
    /// </summary>
    public class Main : INotifyPropertyChanged
    {
        private string _hcePath;
        
        public LogWindow LogWindow { get; set; }

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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}