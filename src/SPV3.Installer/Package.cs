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
        ///     Extraction target directory.
        /// </summary>
        /// <example>
        ///     C:\SPV3.2
        /// </example>
        public Directory Target { get; set; }

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
        ///     Extracts the package data to the target directory.
        ///     A backup routine should be conducted prior to invoking this method, for the purpose of preventing
        ///     collisions with files that may already exist at the target destination.
        /// </summary>
        public void Extract()
        {
            ZipFile.ExtractToDirectory(Path.Value, Target.Path.Value);
        }
    }
}