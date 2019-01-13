/**
 * Copyright (C) 2018-2019 Emilian Roman
 * 
 * This file is part of HCE.HCE.BalsamV.
 * 
 * HCE.HCE.BalsamV is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * HCE.HCE.BalsamV is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with HCE.HCE.BalsamV.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;

namespace HCE.BalsamV.Input
{
    /// <summary>
    ///     Representation of the profile mouse sensitivity settings.
    /// </summary>
    public class Sensitivity
    {
        /// <summary>
        ///     Horizontal sensitivity value.
        ///     This value is expected to be between 1 and 10.
        /// </summary>
        private byte _horizontal = 3;

        /// <summary>
        ///     Vertical sensitivity value.
        ///     This value is expected to be between 1 and 10.
        /// </summary>
        private byte _vertical = 3;

        /// <summary>
        ///     Horizontal sensitivity value.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Assigned sensitivity value is less than 1 or greater than 10.
        /// </exception>
        public byte Horizontal
        {
            get => _horizontal;
            set
            {
                if (!IsValid(value))
                    throw new ArgumentOutOfRangeException(nameof(value),
                        "Assigned sensitivity value is less than 1 or greater than 10.");

                _horizontal = value;
            }
        }

        /// <summary>
        ///     Vertical sensitivity value.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Assigned sensitivity value is less than 1 or greater than 10.
        /// </exception>
        public byte Vertical
        {
            get => _vertical;
            set
            {
                if (!IsValid(value))
                    throw new ArgumentOutOfRangeException(nameof(value),
                        "Assigned sensitivity value is less than 1 or greater than 10.");

                _vertical = value;
            }
        }

        /// <summary>
        ///     Checks if the inbound sensitivity value falls within a valid range.
        /// </summary>
        /// <param name="value">
        ///     Inbound value to check.
        /// </param>
        /// <returns>
        ///     True on valid value, otherwise false.
        /// </returns>
        private static bool IsValid(byte value)
        {
            return value > 1 && value < 11;
        }
    }
}