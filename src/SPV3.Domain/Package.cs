using System;
using System.Collections.Generic;

namespace SPV3.Domain
{
    /// <summary>
    ///     Represents a Package with Entries. <see cref="Entry" />
    /// </summary>
    public class Package
    {
        /// <summary>
        ///     Maximum count allowed for the Entries list.
        /// </summary>
        private const int MaxCount = 0xFF;

        /// <summary>
        ///     <see cref="Entries" />
        /// </summary>
        private List<Entry> _entries;

        /// <summary>
        ///     <see cref="Name" />
        /// </summary>
        public Name Name { get; set; }

        /// <summary>
        ///     List of entries in the package.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Entries count exceeds upper bound. <see cref="MaxCount" />
        /// </exception>
        public List<Entry> Entries
        {
            get => _entries;
            set
            {
                if (value.Count > MaxCount)
                {
                    var message = $"Entries count exceeds upper bound of {MaxCount}.";
                    throw new ArgumentOutOfRangeException(nameof(value), message);
                }

                _entries = value;
            }
        }

        /// <summary>
        ///     Represent object as string.
        /// </summary>
        /// <param name="package">
        ///     Object to represent as string.
        /// </param>
        /// <returns>
        ///     String representation of the object.
        /// </returns>
        public static implicit operator string(Package package)
        {
            return package.Name;
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
        public static explicit operator Package(string value)
        {
            return new Package
            {
                Name = (Name) value
            };
        }

        /// <summary>
        ///     Represent object as Entry list.
        /// </summary>
        /// <param name="package">
        ///     Object to represent as Entry list.
        /// </param>
        /// <returns>
        ///     Entry list representation of the object.
        /// </returns>
        public static implicit operator List<Entry>(Package package)
        {
            return package.Entries;
        }

        /// <summary>
        ///     Represent Entry list as object.
        /// </summary>
        /// <param name="value">
        ///     Entry list to represent as object.
        /// </param>
        /// <returns>
        ///     Object representation of the Entry list.
        /// </returns>
        public static explicit operator Package(List<Entry> value)
        {
            return new Package
            {
                Entries = value
            };
        }
    }
}