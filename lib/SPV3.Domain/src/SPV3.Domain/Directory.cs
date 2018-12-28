namespace SPV3.Domain
{
    /// <summary>
    ///     Represents a directory on the filesystem.
    /// </summary>
    public class Directory
    {
        /// <summary>
        ///     <see cref="Name" />
        /// </summary>
        public Name Name { get; set; }

        /// <summary>
        ///     Represent object as string.
        /// </summary>
        /// <param name="directory">
        ///     Object to represent as string.
        /// </param>
        /// <returns>
        ///     String representation of the object.
        /// </returns>
        public static implicit operator string(Directory directory)
        {
            return directory.Name;
        }

        /// <summary>
        ///     Represent string as object.
        /// </summary>
        /// <param name="name">
        ///     String to represent as object.
        /// </param>
        /// <returns>
        ///     Object representation of the string.
        /// </returns>
        public static explicit operator Directory(string name)
        {
            return new Directory
            {
                Name = (Name) name
            };
        }
    }
}