using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using Directory = SPV3.Domain.Directory;
using File = SPV3.Domain.File;

namespace SPV3.Compiler
{
    /// <summary>
    ///     Creates DEFLATE archives for files/directories on the filesystem.
    /// </summary>
    public class Compressor
    {
        private const CompressionLevel Level = CompressionLevel.Optimal;

        /// <summary>
        ///     Creates a DEFLATE archive for the provided source Directory at the given target.
        /// </summary>
        /// <param name="target">
        ///     Target archive which will contain the compressed source Directory.
        /// </param>
        /// <param name="source">
        ///     Source Directory on the filesystem to compress to the target archive.
        /// </param>
        public void Compress(File target, Directory source)
        {
            ZipFile.CreateFromDirectory(source, target, Level, true);
        }

        /// <summary>
        ///     Creates a DEFLATE archive for the provided files in source Directory at the given target.
        /// </summary>
        /// <param name="target">
        ///     Target archive which will contain the compressed source Directory.
        /// </param>
        /// <param name="source">
        ///     Source Directory on the filesystem containing the files.
        /// </param>
        /// <param name="files">
        ///     Files in the source Directory to compress to the target archive.
        /// </param>
        public void Compress(File target, Directory source, IEnumerable<File> files)
        {
            using (var zip = ZipFile.Open(target, ZipArchiveMode.Create))
            {
                foreach (var file in files)
                    zip.CreateEntryFromFile(Path.Combine(source, file), file, Level);
            }
        }
    }
}