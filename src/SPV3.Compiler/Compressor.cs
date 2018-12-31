using System.Collections.Generic;
using SPV3.Domain;

namespace SPV3.Compiler
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