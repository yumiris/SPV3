namespace SPV3.Installer
{
    /// <summary>
    ///     Represents an SPV3-related directory on the filesystem.
    /// </summary>
    public class Directory
    {
        /// <example>
        ///     maps
        /// </example>
        public Name Name { get; set; }

        /// <example>
        ///     C:\SPV3.2\maps
        /// </example>
        public Path Path { get; set; }
    }
}