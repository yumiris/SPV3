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

        private readonly ILogger _logger;

        public Package(string archiveName, string description, string destination)
        {
            ArchiveName = archiveName;
            Description = description;
            Destination = destination;
        }

        public Package(string archiveName, string description, string destination, ILogger logger)
            : this(archiveName, description, destination)
        {
            _logger = logger;
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
        /// False if:
        /// - Package archive does not exist.
        /// - Install destination does not exist.
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
        /// <exception cref="PackageException">
        ///     Package archive does not exist.
        ///     - or -
        ///     Destination directory does not exist.
        /// </exception>
        public void Install()
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
                if (_logger != null)
                    _logger.Log($"{Description} data already exists. This is fine!");
                else
                    throw;
            }

            _logger?.Log($"{Description} data has been installed successfully to the filesystem.");
        }
    }
}