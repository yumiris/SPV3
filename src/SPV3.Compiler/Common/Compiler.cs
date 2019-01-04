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

using SPV3.Domain;

namespace SPV3.Compiler.Common
{
    /// <summary>
    ///     Creates DEFLATE packages and installation manifest file.
    /// </summary>
    public abstract class Compiler
    {
        /// <summary>
        ///     Prefix used for the package/manifest files.
        /// </summary>
        protected const string Prefix = "0x";

        /// <summary>
        ///     Suffix (or extension) used for the package/manifest files.
        /// </summary>
        protected const string Suffix = ".bin";

        /// <summary>
        ///     Compressed used for creating the package files.
        /// </summary>
        protected readonly Compressor Compressor;

        /// <summary>
        ///     Status object for outputting compilation progress.
        /// </summary>
        protected readonly IStatus Status;

        /// <summary>
        ///     Compiler constructor.
        /// </summary>
        /// <param name="compressor">Compressed used for creating the package files.</param>
        /// <param name="status">Status object for outputting compilation progress.</param>
        protected Compiler(Compressor compressor, IStatus status = null)
        {
            Compressor = compressor;
            Status = status;
        }

        /// <summary>
        ///     Compiles the Source directory to Package files in the Target directory.
        /// </summary>
        /// <param name="source">Directory containing SPV3/HCE data.</param>
        /// <param name="target">Directory to create the package files in.</param>
        /// <returns>Manifest of the compiled packages.</returns>
        public abstract Manifest Compile(Directory source, Directory target);

        /// <summary>
        ///     Wrapper for IStatus .CommitStatus().
        /// </summary>
        /// <param name="text">
        ///     Text to commit to the IStatus instance.
        /// </param>
        protected void Notify(string text)
        {
            Status?.CommitStatus(text);
        }
    }
}
