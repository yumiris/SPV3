namespace SPV3.Installer
{
    /// <summary>
    ///     Type representing an SPV3 file (e.g. map, dynamic library, etc.).
    /// </summary>
    public class File
    {
        /// <summary>
        ///     Name of the file.
        /// </summary>
        /// <example>
        ///     0x01.pkg
        /// </example>
        public Name Name { get; set; }

        /// <summary>
        ///     Absolute path of the file.
        /// </summary>
        /// <example>
        ///     C:\0x01.pkg
        /// </example>
        public Path Path { get; set; }

        /// <summary>
        ///     Description of the file.
        /// </summary>
        /// <example>
        ///     SPV3.2 Core Installation Data
        /// </example>
        public Description Description { get; set; }
    }
}