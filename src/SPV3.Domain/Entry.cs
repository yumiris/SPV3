namespace SPV3.Domain
{
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
        /// <param name="value">
        ///     String to represent as object.
        /// </param>
        /// <returns>
        ///     Object representation of the string.
        /// </returns>
        public static explicit operator Entry(string value)
        {
            return new Entry
            {
                Name = (Name) value
            };
        }
    }
}