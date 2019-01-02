/**
 * Copyright (C) 2019 Emilian Roman
 * 
 * This file is part of SPV3.Loader.
 * 
 * SPV3.Loader is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * SPV3.Loader is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with SPV3.Loader.  If not, see <http://www.gnu.org/licenses/>.
 */

using System.IO;

namespace SPV3.Loader
{
    /// <summary>
    ///     Representation of a valid HCE executable.
    /// </summary>
    public class Executable
    {
        /// <summary>
        ///     Official name of the HCE executable.
        /// </summary>
        public const string Name = "haloce.exe";

        /// <summary>
        ///     Byte length of the HCE executable. 
        /// </summary>
        private const int Length = 0x24B000;

        /// <summary>
        ///     Executable constructor.
        /// </summary>
        /// <param name="path">
        ///     <see cref="Path" />
        /// </param>
        public Executable(string path)
        {
            Path = path;
        }

        /// <summary>
        ///     Absolute path to the HCE executable.
        /// </summary>
        public string Path { get; }

        /// <summary>
        ///     Compares the executable on the filesystem against the specifications of a valid HCE executable.
        /// </summary>
        /// <returns>
        ///     True on the executable matching a valid HCE executable; otherwise false.
        /// </returns>
        public bool Verify()
        {
            if (!File.Exists(Path))
                throw new FileNotFoundException("Cannot verify non-existent HCE executable file.");

            return new FileInfo(Path).Length == Length;
        }
    }
}