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
using System.IO;
using System.IO.Compression;
using SPV3.Compiler.Common;
using Directory = SPV3.Domain.Directory;
using File = SPV3.Domain.File;

namespace SPV3.Compiler.Compressors
{
    /// <inheritdoc />
    public class InternalCompressor : Compressor
    {
        /// <inheritdoc />
        public override void Compress(File target, Directory source)
        {
            ZipFile.CreateFromDirectory(source, target);
        }

        /// <inheritdoc />
        public override void Compress(File target, Directory source, IEnumerable<File> whitelist)
        {
            using (var zip = ZipFile.Open(target, ZipArchiveMode.Create))
            {
                const CompressionLevel level = CompressionLevel.Optimal;

                foreach (var file in whitelist)
                    zip.CreateEntryFromFile(Path.Combine(source, file), file, level);
            }
        }
    }
}
