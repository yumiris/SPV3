using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using Directory = SPV3.Domain.Directory;
using File = SPV3.Domain.File;

namespace SPV3.Compiler
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
        public override void Compress(File target, Directory source, IEnumerable<File> files)
        {
            using (var zip = ZipFile.Open(target, ZipArchiveMode.Create))
            {
                const CompressionLevel level = CompressionLevel.Optimal;

                foreach (var file in files)
                    zip.CreateEntryFromFile(Path.Combine(source, file), file, level);
            }
        }
    }
}