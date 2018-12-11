using System.Collections.Generic;

namespace SPV3.Installer
{
    /// <summary>
    ///     Conducts package backup & extraction routines.
    /// </summary>
    public class Installer
    {
        /// <summary>
        ///     Packages to install on the filesystem.
        /// </summary>
        public List<Package> Packages { get; set; }

        /// <summary>
        ///     Backup service to inject into the packages upon extraction.
        /// </summary>
        public Backup Backup { get; set; }

        /// <summary>
        ///     Installs the specified packages to the target directory.
        /// </summary>
        /// <param name="directory">
        ///     Directory containing all of the extracted packages' files.
        /// </param>
        public void InstallTo(Directory directory)
        {
            foreach (var package in Packages)
            {
                Backup.CommitOn(package);
                package.ExtractTo(directory);
            }
        }

        /// <summary>
        ///     Implicitly represents object as a Package list.
        /// </summary>
        /// <param name="installer">
        ///     Object instance.
        /// </param>
        /// <returns>
        ///     Installer.Packages
        /// </returns>
        public static implicit operator List<Package>(Installer installer)
        {
            return installer.Packages;
        }
    }
}