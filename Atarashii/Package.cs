using System.IO;
using System.IO.Compression;
using Atarashii.Exceptions;

namespace Atarashii
{
    /// <summary>
    ///     Archive installer and verifier.
    /// </summary>
    public class Package
    {
        /// <summary>
        ///     Directory containing the expected packages.
        /// </summary>
        public const string Directory = "Packages";

        /// <summary>
        ///     Package archive extension.
        /// </summary>
        public const string Extension = "pkg";

        public Package(string archiveName, string description, string destination)
        {
            ArchiveName = archiveName + $".{Extension}";
            Description = description;
            Destination = destination;
        }

        /// <summary>
        ///     Name of the archive file without any extensions or paths.
        /// </summary>
        public string ArchiveName { get; }

        /// <summary>
        ///     Informative line about the package.
        /// </summary>
        public string Description { get; }

        /// <summary>
        ///     Destination directory path for the installed contents.
        /// </summary>
        public string Destination { get; }

        /// <summary>
        ///     Applies the files in the package to the destination on the filesystem.
        /// </summary>
        /// <exception cref="PackageException">
        ///     Package archive does not exist.
        /// </exception>
        /// <exception cref="PackageException">
        ///     Destination directory does not exist.
        /// </exception>
        public void Install()
        {
            string package = Path.Combine(Directory, ArchiveName);

            if (!File.Exists(package))
                throw new PackageException("Package archive does not exist.");

            if (!System.IO.Directory.Exists(Destination))
                throw new PackageException("Destination does not exist.");

            ZipFile.ExtractToDirectory(package, Destination);
        }
    }
}