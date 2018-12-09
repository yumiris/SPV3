using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace SPV3.Installer
{
    /// <summary>
    ///     Represents an archive of files which are all installed to a specific location.
    /// </summary>
    public class Package
    {
        /// <summary>
        ///     Extension used for the package file on the filesystem.
        /// </summary>
        public const string Extension = ".pkg";

        /// <summary>
        ///     Package filename on the system.
        /// </summary>
        /// <example>
        ///     0x01.pkg
        /// </example>
        public Name Name { get; set; }

        /// <summary>
        ///     Package description for the end-user.
        /// </summary>
        /// <example>
        ///     SPV3.2 Core Installation Data
        /// </example>
        public Description Description { get; set; }

        /// <summary>
        ///     Optional subdirectory for package files.
        /// </summary>
        public Directory Directory { get; set; }

        /// <summary>
        ///     Files contained within the package.
        /// </summary>
        /// <example>
        ///     {
        ///     haloce.exe,
        ///     spv3.exe
        ///     dinput8.dll
        ///     }
        /// </example>
        public List<File> Files { get; set; }

        /// <summary>
        ///     Checks if the package exists on the filesystem using the Path value.
        /// </summary>
        /// <returns>
        ///     True if package exists, otherwise false.
        /// </returns>
        public bool Exists()
        {
            return System.IO.File.Exists(Name.Value);
        }

        /// <summary>
        ///     Extracts the package data to the inbound directory.
        /// </summary>
        /// <remarks>
        ///     If the Directory property is defined, then the package files will be dedicated to a subdirectory in the
        ///     inbound directory.
        /// </remarks>
        /// <remarks>
        ///     A backup routine should be conducted prior to invoking this method, for the purpose of preventing
        ///     collisions with files that may already exist at the target destination.
        /// </remarks>
        /// <param name="directory"></param>
        /// <exception cref="FileNotFoundException">
        ///     Package does not exist on the filesystem.
        /// </exception>
        /// <exception cref="DirectoryNotFoundException">
        ///     Target directory does not exist on the filesystem.
        /// </exception>
        public void ExtractTo(Directory directory)
        {
            if (!Exists())
                throw new FileNotFoundException("Package does not exist on the filesystem.");

            if (!directory.Exists())
                throw new DirectoryNotFoundException("Target directory does not exist on the filesystem.");

            /**
             * If this package instance has the Directory property defined, then the inbound directory will be mutated
             * into <inbound directory> + <package directory> for the purpose of extracting the package files into a
             * subdirectory.
             */
            if (Directory != null)
            {
                directory.Name.Value = Path.Combine(directory.Name.Value, Directory.Name.Value);
                directory.Create();
            }

            ZipFile.ExtractToDirectory(Name.Value, directory.Name.Value);
        }
    }
}