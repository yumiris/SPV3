using Atarashii.GUI;

namespace Atarashii.OpenSauce.GUI
{
    /// <summary>
    ///     Model for installing OpenSauce
    /// </summary>
    public class Main : BaseModel
    {
        private bool _canInstall;
        private string _installationPath;

        /// <summary>
        ///     HCE installation path for OpenSauce libraries
        /// </summary>
        public string InstallationPath
        {
            get => _installationPath;
            set
            {
                if (value == _installationPath) return;
                _installationPath = value;
                OnPropertyChanged();
                CheckCanInstall();
            }
        }

        /// <summary>
        ///     OpenSauce can be installed on the system
        /// </summary>
        public bool CanInstall
        {
            get => _canInstall;
            set
            {
                if (value == _canInstall) return;
                _canInstall = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Invokes the OpenSauce installation procedure
        /// </summary>
        public void InstallOpenSauce()
        {
            new InstallerFactory(InstallationPath, LogWindow).Get().Install();
        }

        /// <summary>
        ///     Invokes the OpenSauce installation verification
        /// </summary>
        private void CheckCanInstall()
        {
            var state = new InstallerFactory(InstallationPath).Get().Verify();
            CanInstall = state.IsValid;
            LogWindow.Log(CanInstall ? "Ready to install!" : state.Reason);
        }
    }
}