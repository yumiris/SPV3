using System.IO;
using Microsoft.Win32;

namespace Atarashii.Executable
{
    /// <summary>
    ///     Attempts to retrieve the path of the HCE executable on the system.
    /// </summary>
    public class Detector
    {
        /// <summary>
        ///     HCE executable name.
        /// </summary>
        private const string ExecutableName = @"haloce.exe";

        /// <summary>
        ///     Default location set by the HCE installer.
        /// </summary>
        private const string DefaultInstall = @"C:\Program Files (x86)\Halo Custom Edition";

        /// <summary>
        ///     HCE registry keys location.
        /// </summary>
        private const string RegKeyLocation = @"SOFTWARE\Microsoft\Microsoft Games\Halo CE";

        /// <summary>
        ///     HCE executable path registry key name.
        /// </summary>
        private const string RegKeyIdentity = @"EXE Path";

        /// <summary>
        ///     Retrieves the path of the HCE executable on the system.
        /// </summary>
        /// <returns>
        ///    Absolute executable path if found, otherwise an empty string.
        /// </returns>
        public string Detect()
        {
            using (var view = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
            using (var key = view.OpenSubKey(RegKeyLocation))
            {
                var path = key?.GetValue(RegKeyIdentity);
                if (path != null)
                {
                    return $@"{path}\{ExecutableName}";
                }
            }

            var fullDefaultPath = $@"{DefaultInstall}\{ExecutableName}";

            if (File.Exists(fullDefaultPath))
            {
                return fullDefaultPath;
            }

            var currentDirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), ExecutableName);
            return File.Exists(ExecutableName) ? currentDirectoryPath : string.Empty;
        }
    }
}