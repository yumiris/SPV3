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

namespace SPV3.Compiler.Common
{
    /// <summary>
    ///     Creates DEFLATE archives for files/directories on the filesystem.
    /// </summary>
    public abstract class Compressor
    {
        /// <summary>
        ///     Creates a DEFLATE archive for the provided source Directory at the given target.
        /// </summary>
        /// <param name="target">
        ///     Target archive which will contain the compressed source Directory.
        /// </param>
        /// <param name="source">
        ///     Source Directory on the filesystem to compress to the target archive.
        /// </param>
        public abstract void Compress(File target, Directory source);

        /// <summary>
        ///     Creates a DEFLATE archive for the provided files in source Directory at the given target.
        /// </summary>
        /// <param name="target">
        ///     Target archive which will contain the compressed source Directory.
        /// </param>
        /// <param name="source">
        ///     Source Directory on the filesystem containing the files.
        /// </param>
        /// <param name="whitelist">
        ///     Files in the source Directory to compress to the target archive.
        /// </param>
        public abstract void Compress(File target, Directory source, IEnumerable<File> whitelist);
    }
}
