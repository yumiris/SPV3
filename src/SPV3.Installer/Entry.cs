using SPV3.Domain;

namespace SPV3.Installer
{
    /// <summary>
    ///     Type of Entry on the filesystem.
    /// </summary>
    public enum EntryType
    {
        /// <summary>
        ///     Entry is unknown. Does filesystem record exist?
        /// </summary>
        Unknown,
        
        /// <summary>
        ///     Entry is a binary or text file.
        /// </summary>
        File,

        /// <summary>
        ///     Entry is a directory.
        /// </summary>
        Directory
    }

    /// <summary>
    ///     Represents an entry within a Package. <see cref="Package" />
    /// </summary>
    public class Entry
    {
        /// <summary>
        ///     <see cref="Name" />
        /// </summary>
        public Name Name { get; set; }

        /// <summary>
        ///     <see cref="EntryType" />
        /// </summary>
        public EntryType Type { get; set; } = EntryType.Unknown;

        /// <summary>
        ///     Represent object as string.
        /// </summary>
        /// <param name="entry">
        ///     Object to represent as string.
        /// </param>
        /// <returns>
        ///     String representation of the object.
        /// </returns>
        public static implicit operator string(Entry entry)
        {
            return entry.Name;
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
        public static explicit operator Entry(string name)
        {
            return new Entry
            {
                Name = (Name) name
            };
        }
    }
}