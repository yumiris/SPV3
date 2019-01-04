/**
 * Copyright (C) 2019 Emilian Roman
 * 
 * This file is part of SPV3.Domain.
 * 
 * SPV3.Domain is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * SPV3.Domain is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with SPV3.Domain.  If not, see <http://www.gnu.org/licenses/>.
 */

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
        ///     Optional description for the Package.
        ///     <see cref="Description" />
        /// </summary>
        public Description Description { get; set; }

        /// <summary>
        ///     Optional directory which the Package represents.
        ///     <see cref="Directory" />
        /// </summary>
        public Directory Directory { get; set; }

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
