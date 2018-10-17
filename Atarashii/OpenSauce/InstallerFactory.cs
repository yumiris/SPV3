using System;
using System.Collections.Generic;
using System.IO;

namespace Atarashii.OpenSauce
{
    public class InstallerFactory
    {
        public enum Type
        {
            Default
        }

        private readonly string _installationPath;
        private readonly ILogger _logger;

        /// <summary>
        ///     OpenSauceInstallerFactory constructor.
        /// </summary>
        /// <param name="installationPath">
        ///     The HCE directory path -- used to install the OpenSauce library data to.
        /// </param>
        public InstallerFactory(string installationPath)
        {
            _installationPath = installationPath;
        }

        /// <inheritdoc />
        /// <param name="installationPath">
        ///     The HCE directory path -- used to install the OpenSauce library data to.
        /// </param>
        /// <param name="logger">
        ///     Logging class for logging the packages' output.
        /// </param>
        public InstallerFactory(string installationPath, ILogger logger) : this(installationPath)
        {
            _logger = logger;
        }

        /// <summary>
        ///     Retrieves OpenSauceInstaller instance.
        /// </summary>
        /// <param name="type">
        ///     Type of OpenSauceInstaller instance.
        /// </param>
        /// <returns>
        ///     OpenSauceInstaller instance for installing OpenSauce to the filesystem with the built-in packages.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Invalid OpenSauceInstaller type given.
        /// </exception>
        public Installer Get(Type type = Type.Default)
        {
            switch (type)
            {
                case Type.Default:
                    return new Installer(_installationPath, GetOpenSaucePackages());
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        /// <summary>
        ///     Retrieve a list of packages that represent the OpenSauce installation data.
        /// </summary>
        /// <returns>
        ///     A list of OpenSauce packages that replicate an original OS installation when installed.
        ///     All of the packages are expected to be in the directory defined by the Package.Directory constant.
        /// </returns>
        private List<Package> GetOpenSaucePackages()
        {
            var guiDirPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            var usrDirPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

            var libPackage = Path.Combine(Package.Directory, "lib.pkg");
            var guiPackage = Path.Combine(Package.Directory, "gui.pkg");
            var usrPackage = Path.Combine(Package.Directory, "usr.pkg");

            return new List<Package>
            {
                new Package(libPackage, "OpenSauce core and dependencies", _installationPath, _logger),
                new Package(guiPackage, "In-game OpenSauce UI assets", guiDirPath, _logger),
                new Package(usrPackage, "OpenSauce XML user configuration", usrDirPath, _logger)
            };
        }
    }
}