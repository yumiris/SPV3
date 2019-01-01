/**
 * Copyright (C) 2019 Emilian Roman
 * 
 * This file is part of SPV3.Installer.
 * 
 * SPV3.Installer is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * SPV3.Installer is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with SPV3.Installer.  If not, see <http://www.gnu.org/licenses/>.
 */


using SPV3.Domain;

namespace SPV3.Installer.Domain
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
