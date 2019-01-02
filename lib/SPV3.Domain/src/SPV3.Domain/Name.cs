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

namespace SPV3.Domain
{
    /// <summary>
    ///     Serves as an identifier for a file or directory.
    /// </summary>
    public class Name
    {
        /// <summary>
        ///     Maximum length allowed for the provided value.
        /// </summary>
        public const int MaxLength = 0xFF;

        /// <summary>
        ///     <see cref="Value" />
        /// </summary>
        private string _value;

        /// <summary>
        ///     Name value.
        /// </summary>
        /// <example>
        ///     Implicit declaration.
        /// </example>
        /// <code>
        ///    var hceName = (Name) "haloce.exe"
        /// </code>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Value length exceeds upper bound. <see cref="MaxLength" />
        /// </exception>
        public string Value
        {
            get => _value;
            set
            {
                if (value.Length > MaxLength)
                {
                    var message = $"Value '{value}' length exceeds upper bound of {MaxLength}.";
                    throw new ArgumentOutOfRangeException(nameof(value), message);
                }

                _value = value;
            }
        }

        /// <summary>
        ///     Represent object as string.
        /// </summary>
        /// <param name="name">
        ///     Object to represent as string.
        /// </param>
        /// <returns>
        ///     String representation of the object.
        /// </returns>
        public static implicit operator string(Name name)
        {
            return name.Value;
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
        public static explicit operator Name(string value)
        {
            return new Name
            {
                Value = value
            };
        }
    }
}