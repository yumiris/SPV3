namespace SPV3.Installer
{
    /// <summary>
    ///     Represents an SPV3-related directory on the filesystem.
    /// </summary>
    public class Directory
    {
        /// <summary>
        ///     Name of the directory.
        /// </summary>
        /// <example>
        ///     maps
        /// </example>
        public Name Name { get; set; }

        /// <summary>
        ///     Absolute path of the directory.
        /// </summary>
        /// <example>
        ///     C:\SPV3.2\maps
        /// </example>
        public Path Path { get; set; }

        /// <summary>
        ///     Checks if the directory exists on the filesystem using the Path value.
        /// </summary>
        /// <returns>
        ///     True if directory exists, otherwise false.
        /// </returns>
        public bool Exists()
        {
            return System.IO.Directory.Exists(Path.Value);
        }

        /// <summary>
        ///     Creates the directory at the given path value if it does not exist on the filesystem.
        /// </summary>
        public void Create()
        {
            if (!Exists())
                System.IO.Directory.CreateDirectory(Path.Value);
        }
    }
}