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

namespace HCE.BalsamV.Profile
{
    /// <summary>
    ///     Representation of the profile player name.
    /// </summary>
    public class Name
    {
        /// <summary>
        ///     Player name value.
        ///     This value is expected to be between 1 and 11 characters.
        /// </summary>
        private string _value = "New001";

        /// <summary>
        ///     Player name value.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     No name value has been been assigned.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Assigned name value is greater than 11 characters.
        /// </exception>
        public string Value
        {
            get => _value;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException(value);

                if (value.Length > 0xB)
                    throw new ArgumentOutOfRangeException(nameof(value),
                        "Assigned name value is greater than 11 characters.");

                _value = value;
            }
        }
    }
}