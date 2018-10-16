using System;
using System.Collections.Generic;

namespace Atarashii
{
    public class PackageFactory
    {
        /// <summary>
        ///     Builds a list of packages that represent the OpenSauce installation data.
        /// </summary>
        /// <param name="installationPath">
        ///     The HCE directory path -- used to install the OpenSauce library data to.
        /// </param>
        /// <param name="logger">
        ///     Optional logging class for logging the packages' output.
        /// </param>
        /// <returns>
        ///     A list of OpenSauce packages that replicate an original OS installation when installed.
        /// </returns>
        public static List<Package> GetOpenSaucePackages(string installationPath, ILogger logger = null)
        {
            var guiDirPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            var usrDirPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

            return new List<Package>
            {
                new Package("lib.pkg", "OpenSauce core and dependencies", installationPath, logger),
                new Package("gui.pkg", "In-game OpenSauce UI assets", guiDirPath, logger),
                new Package("usr.pkg", "OpenSauce XML user configuration", usrDirPath, logger)
            };
        }
    }
}