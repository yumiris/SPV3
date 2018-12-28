namespace SPV3.Domain
{
    /// <summary>
    ///     Represents a file on the filesystem.
    /// </summary>
    public class File
    {
        /// <summary>
        ///     <see cref="Name" />
        /// </summary>
        public Name Name { get; set; }

        /// <summary>
        ///     Represent object as string.
        /// </summary>
        /// <param name="file">
        ///     Object to represent as string.
        /// </param>
        /// <returns>
        ///     String representation of the object.
        /// </returns>
        public static implicit operator string(File file)
        {
            return file.Name;
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
        public static explicit operator File(string name)
        {
            return new File
            {
                Name = (Name) name
            };
        }
    }
}