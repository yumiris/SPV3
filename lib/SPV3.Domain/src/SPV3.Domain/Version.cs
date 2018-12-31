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

namespace SPV3.Domain
{
    /// <summary>
    ///     Type for storing a semantic version value.
    /// </summary>
    public class Version
    {
        /// <summary>
        ///     <see cref="Major" />
        /// </summary>
        private int _major;

        /// <summary>
        ///     <see cref="Minor" />
        /// </summary>
        private int _minor;

        /// <summary>
        ///     <see cref="Patch" />
        /// </summary>
        private int _patch;

        /// <summary>
        ///     Value representing an API-breaking version change.
        /// </summary>
        public int Major
        {
            get => _major;
            set => _major = value;
        }

        /// <summary>
        ///     Value representing an API addition or change.
        /// </summary>
        public int Minor
        {
            get => _minor;
            set => _minor = value;
        }

        /// <summary>
        ///     Value representing a fix or patch.
        /// </summary>
        public int Patch
        {
            get => _patch;
            set => _patch = value;
        }

        /// <summary>
        ///     Represent object as string.
        /// </summary>
        /// <param name="version">
        ///     Object to represent as string.
        /// </param>
        /// <returns>
        ///     String representation of the object.
        /// </returns>
        public static implicit operator string(Version version)
        {
            return $"{version.Major}.{version.Minor}.{version.Patch}";
        }

        /// <summary>
        ///     Represent string as object.
        /// </summary>
        /// <param name="version">
        ///     String to represent as object.
        /// </param>
        /// <returns>
        ///     Object representation of the string.
        /// </returns>
        public static explicit operator Version(string version)
        {
            var split = version.Split('.');

            return new Version
            {
                Major = int.Parse(split[0]),
                Minor = int.Parse(split[1]),
                Patch = int.Parse(split[2])
            };
        }
    }
}