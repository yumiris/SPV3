namespace SPV3.Installer
{
    /// <summary>
    ///     Represents an SPV3-related directory on the filesystem.
    /// </summary>
    public class Directory
    {
        /// <summary>
        ///     Name of the directory on the filesystem.
        /// </summary>
        /// <example>
        ///     maps
        /// </example>
        public Name Name { get; set; }

        /// <summary>
        ///     Checks if the directory exists on the filesystem using the Path value.
        /// </summary>
        /// <returns>
        ///     True if directory exists, otherwise false.
        /// </returns>
        public bool Exists()
        {
            return System.IO.Directory.Exists(Name.Value);
        }

        /// <summary>
        ///     Creates the directory at the given path value if it does not exist on the filesystem.
        /// </summary>
        public void Create()
        {
            if (!Exists())
                System.IO.Directory.CreateDirectory(Name.Value);
        }

        /// <summary>
        ///     Implicitly represents object as string.
        /// </summary>
        /// <param name="directory">
        ///     Object instance.
        /// </param>
        /// <returns>
        ///     Directory.Name
        /// </returns>
        public static implicit operator string(Directory directory)
        {
            return directory.Name;
        }

        /// <summary>
        ///     Explicitly represents string as object.
        /// </summary>
        /// <param name="value">
        ///     Name.Value
        /// </param>
        /// <returns>
        ///     Object instance.
        /// </returns>
        public static explicit operator Directory(string value)
        {
            return new Directory
            {
                Name = new Name
                {
                    Value = value
                }
            };
        }
    }
}