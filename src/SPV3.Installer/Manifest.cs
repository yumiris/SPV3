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

using System.Collections.Generic;
using SPV3.Domain;

namespace SPV3.Installer
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