using System.IO;
using System.IO.Compression;
using Atarashii.Exceptions;

namespace Atarashii
{
    /// <summary>
    ///     Archive installer and verifier.
    /// </summary>
    public class Package : Module, IVerifiable
    {
        protected override string Identifier { get; } = "Atarashii.Package";

        /// <summary>
        ///     Directory containing the expected packages.
        /// </summary>
        public const string Directory = "Packages";

        /// <summary>
        ///     Package archive extension.
        /// </summary>
        public const string Extension = "pkg";

        private readonly Output _output;

        public Package(string archiveName, string description, string destination)
        {
            ArchiveName = archiveName;
            Description = description;
            Destination = destination;
        }

        public Package(string archiveName, string description, string destination, Output output)
            : this(archiveName, description, destination)
        {
            _output = output;
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
            WriteInfo($"Verifying {Description} package.");
            var state = Verify();

            if (!state.IsValid)
                WriteAndThrow(new PackageException(state.Reason));

            WriteSuccess($"Package {Description} has been successfully verified.");

            try
            {
                ZipFile.ExtractToDirectory(ArchiveName, Destination);
            }
            catch (IOException)
            {
                WriteInfo($"{Description} data already exists. This is fine!");
            }
            
            WriteSuccess($"{Description} data has been installed successfully to the filesystem.");
        }
    }
}