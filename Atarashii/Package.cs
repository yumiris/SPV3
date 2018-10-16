using System.IO;
using System.IO.Compression;
using Atarashii.Exceptions;

namespace Atarashii
{
    /// <summary>
    ///     Archive installer and verifier.
    /// </summary>
    public class Package : IVerifiable
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

        /// <inheritdoc />
        public Verification Verify()
        {
            if (!File.Exists(ArchiveName))
                return new Verification(false, "Cannot install specified package. Package archive does not exist.");

            if (!System.IO.Directory.Exists(Destination))
                return new Verification(false, "Cannot install specified package. Destination does not exist.");

            return new Verification(true);
        }

        /// <summary>
        ///     Applies the files in the package to the destination on the filesystem.
        /// </summary>
        /// <param name="logger">
        ///     Logging instance for appending package installation progress.
        /// </param>
        /// <exception cref="PackageException">
        ///     Package archive does not exist.
        ///     - or -
        ///     Destination directory does not exist.
        /// </exception>
        public void Install(ILogger logger)
        {
            var state = Verify();

            if (!state.IsValid)
                throw new PackageException(state.Reason);

            try
            {
                ZipFile.ExtractToDirectory(ArchiveName, Destination);
            }
            catch (IOException)
            {
                logger.Log($"{Description} data already exists. This is fine!");
            }

            logger.Log($"{Description} data has been installed successfully to the filesystem.");
        }
    }
}