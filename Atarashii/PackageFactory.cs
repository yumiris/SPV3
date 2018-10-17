using System;
using System.Collections.Generic;
using System.IO;

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
        ///     All of the packages are expected to be in the directory defined by the Package.Directory constant.
        /// </returns>
        public static List<Package> GetOpenSaucePackages(string installationPath, ILogger logger = null)
        {
            var guiDirPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            var usrDirPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

            var libPackage = Path.Combine(Package.Directory, "lib.pkg");
            var guiPackage = Path.Combine(Package.Directory, "gui.pkg");
            var usrPackage = Path.Combine(Package.Directory, "usr.pkg");

            return new List<Package>
            {
                new Package(libPackage, "OpenSauce core and dependencies", installationPath, logger),
                new Package(guiPackage, "In-game OpenSauce UI assets", guiDirPath, logger),
                new Package(usrPackage, "OpenSauce XML user configuration", usrDirPath, logger)
            };
        }
    }
}