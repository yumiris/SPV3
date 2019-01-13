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

namespace HCE.BalsamV
{
    /// <summary>
    ///     This type is used to represent a lastprof.txt text file.
    /// </summary>
    public class Lastprof
    {
        /// <summary>
        ///     Separation character which is guaranteed to be present.
        /// </summary>
        public const char Delimiter = '\\';

        /// <summary>
        ///     Position of the profile name relative to the end of the split string.
        /// </summary>
        public const int NameOffset = 0x2;

        /// <summary>
        ///     Known string that is present in the lastprof.txt.
        /// </summary>
        public const string Signature = "lam.sav";

        /// <summary>
        ///     Name of the last accessed profile.
        ///     This value is expected to be between 1 and 11 characters.
        /// </summary>
        private string _name;

        /// <summary>
        ///     <see cref="_name" />
        /// </summary>
        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException(value);

                if (value.Length > 0xB)
                    throw new ArgumentOutOfRangeException(nameof(value),
                        "Lastprof.txt name value is greater than 11 characters.");

                _name = value;
            }
        }
    }
}