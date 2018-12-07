using System.Collections.Generic;

namespace SPV3.Installer
{
    /// <summary>
    ///     Represents an archive of files which are all installed to a specific location.
    /// </summary>
    public class Package
    {
        /// <summary>
        ///     Package name, i.e. the package filename.
        /// </summary>
        /// <example>
        ///     0x01.pkg
        /// </example>
        public Name Name { get; set; }

        /// <summary>
        ///     Package absolute path on the filesystem.
        /// </summary>
        /// <example>
        ///     C:\0x01.pkg
        /// </example>
        public Path Path { get; set; }

        /// <summary>
        ///     Package description for the end-user.
        /// </summary>
        /// <example>
        ///     SPV3.2 Core Installation Data
        /// </example>
        public Description Description { get; set; }

        /// <summary>
        ///     Installation target directory.
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
    }
}