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

namespace HCE.BalsamV.Settings
{
    /// <summary>
    ///     Representation of the video effects settings.
    /// </summary>
    public class Effects
    {
        /// <summary>
        ///     Shadow effects toggle.
        /// </summary>
        public bool Shadows { get; set; } = true;

        /// <summary>
        ///     Specular effects toggle.
        /// </summary>
        public bool Specular { get; set; } = true;

        /// <summary>
        ///     Decals effects toggle.
        /// </summary>
        public bool Decals { get; set; } = true;
    }
}