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

namespace HCE.BalsamV.Settings
{
    /// <summary>
    ///     Representation of the audio volume settings.
    /// </summary>
    public class Volume
    {
        /// <summary>
        ///     Effects volume value.
        ///     This value is expected to be between 0 and 10.
        /// </summary>
        private byte _effects = 10;

        /// <summary>
        ///     Master volume value.
        ///     This value is expected to be between 0 and 10.
        /// </summary>
        private byte _master = 10;

        /// <summary>
        ///     Music volume value.
        ///     This value is expected to be between 0 and 10.
        /// </summary>
        private byte _music = 10;

        /// <summary>
        ///     Master volume value.
        ///     <exception cref="ArgumentOutOfRangeException">
        ///         Assigned volume value is greater than 10.
        ///     </exception>
        /// </summary>
        public byte Master
        {
            get => _master;
            set
            {
                if (!IsValid(value))
                    throw new ArgumentOutOfRangeException(nameof(value), "Assigned volume value is greater than 10.");

                _master = value;
            }
        }

        /// <summary>
        ///     Effects volume value.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Assigned volume value is greater than 10.
        /// </exception>
        public byte Effects
        {
            get => _effects;
            set
            {
                if (!IsValid(value))
                    throw new ArgumentOutOfRangeException(nameof(value), "Assigned volume value is greater than 10.");

                _effects = value;
            }
        }

        /// <summary>
        ///     Music volume value.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Assigned volume value is greater than 10.
        /// </exception>
        public byte Music
        {
            get => _music;
            set
            {
                if (!IsValid(value))
                    throw new ArgumentOutOfRangeException(nameof(value), "Assigned volume value is greater than 10.");

                _music = value;
            }
        }

        /// <summary>
        ///     Checks if the inbound volume value falls within a valid range.
        /// </summary>
        /// <param name="value">Inbound value to check.</param>
        /// <returns>True on valid value, otherwise false.</returns>
        private static bool IsValid(byte value)
        {
            return value < 11;
        }
    }
}