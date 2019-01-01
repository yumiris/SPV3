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

using System.Collections.Generic;
using SPV3.Domain;

namespace SPV3.Installer.Domain
{
    /// <summary>
    ///     Type representing the manifest for an SPV3 installation.
    /// </summary>
    public class Manifest
    {
        /// <summary>
        ///     Manifest file name.
        ///     <see cref="Name" />
        /// </summary>
        public static readonly Name Name = (Name) "0x00.bin";

        /// <summary>
        ///     Manifest file version.
        ///     <see cref="Version" />
        /// </summary>
        public Version Version { get; set; }

        /// <summary>
        ///     List of installation packages.
        ///     <see cref="Package" />
        /// </summary>
        public List<Package> Packages { get; set; }

        /// <summary>
        ///     Represent object as Package list.
        /// </summary>
        /// <param name="manifest">
        ///     Object to represent as Package list.
        /// </param>
        /// <returns>
        ///     Package list representation of the object.
        /// </returns>
        public static implicit operator List<Package>(Manifest manifest)
        {
            return manifest.Packages;
        }

        /// <summary>
        ///     Represent Package list as object.
        /// </summary>
        /// <param name="value">
        ///     Package list to represent as object.
        /// </param>
        /// <returns>
        ///     Object representation of the Package list.
        /// </returns>
        public static explicit operator Manifest(List<Package> value)
        {
            return new Manifest
            {
                Packages = value
            };
        }
    }
}
