/**
 * Copyright (c) 2018 Emilian Roman
 * 
 * This software is provided 'as-is', without any express or implied
 * warranty. In no event will the authors be held liable for any damages
 * arising from the use of this software.
 * 
 * Permission is granted to anyone to use this software for any purpose,
 * including commercial applications, and to alter it and redistribute it
 * freely, subject to the following restrictions:
 * 
 * 1. The origin of this software must not be misrepresented; you must not
 *    claim that you wrote the original software. If you use this software
 *    in a product, an acknowledgment in the product documentation would be
 *    appreciated but is not required.
 * 2. Altered source versions must be plainly marked as such, and must not be
 *    misrepresented as being the original software.
 * 3. This notice may not be removed or altered from any source distribution.
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