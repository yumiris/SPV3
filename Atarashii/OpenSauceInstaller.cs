using System.IO;

namespace Atarashii
{
    /// <summary>
    ///     OpenSauce packages installation.
    /// </summary>
    public class OpenSauceInstaller
    {
        /// <summary>
        ///     Installs the OpenSauce libraries to the given HCE directory path.
        /// </summary>
        /// <param name="hcePath">
        ///     A valid HCE directory path.
        /// </param>
        public void InstallTo(string hcePath)
        {
            if (!File.Exists(Path.Combine(hcePath, Executable.Name)))
                throw new OpenSauceException("Invalid HCE directory path.");
        }
    }
}