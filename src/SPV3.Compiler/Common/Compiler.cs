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

using SPV3.Domain;
using SPV3.Installer.Domain;

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