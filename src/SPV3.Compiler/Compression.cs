using System;
using System.IO;
using System.Security;
using System.Security.Cryptography;

namespace SPV3.Compiler
{
    /// <summary>
    ///     Class which conducts data compression by invoking a compressor executable.
    ///     By default, it handles the verification and invocation of the 7-Zip 18.05 (2018-04-30) executable.
    /// </summary>
    public class Compression
    {
        /// <summary>
        ///     Original name of the 7-Zip 18.05 (2018-04-30) executable, from the 7-Zip Extra package.
        /// </summary>
        public const string SevenZipFile = "7za.exe";

        /// <summary>
        ///     Path on the filesystem containing the 7-Zip 18.05 (2018-04-30) executable & DLLs.
        ///     This folder is distributed with the SPV3 Compiler.
        /// </summary>
        public const string SevenZipPath = "7z";

        /// <summary>
        ///     MD5 hash of the 7-Zip 18.05 (2018-04-30) executable.
        /// </summary>
        public const string SevenZipHash = "e93c0c45d84f328afba850e87ac7398c";

        /// <summary>
        ///     Compressor constructor.
        /// </summary>
        /// <param name="path">
        ///     Path to the compressor executable on the filesystem.
        ///     Default value: <see cref="SevenZipPath" /> & <see cref="SevenZipFile" />
        /// </param>
        /// <param name="hash">
        ///     Hash of the compressor executable for pre-execution verification.
        ///     Default value: <see cref="SevenZipHash" />
        /// </param>
        public Compression(string path = null, string hash = null)
        {
            Path = path ?? System.IO.Path.Combine(SevenZipPath, SevenZipFile);
            Hash = hash ?? SevenZipHash;
        }

        /// <summary>
        ///     Path to the compressor executable on the filesystem.
        /// </summary>
        public string Path { get; }

        /// <summary>
        ///     Hash of the compressor executable for pre-execution verification.
        /// </summary>
        public string Hash { get; }

        /// <summary>
        ///     Verifies the hash of the compression executable on the filesystem against the hash provided in the
        ///     constructor.
        /// </summary>
        /// <returns>
        ///    True if the computed hashes match, otherwise false.
        /// </returns>
        /// <exception cref="FileNotFoundException">
        ///    Compression executable does not exist.
        /// </exception>
        public bool Verify()
        {
            if (!File.Exists(Path))
                throw new FileNotFoundException("Compression executable does not exist.");

            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(Path))
                {
                    return BitConverter
                        .ToString(md5.ComputeHash(stream))
                        .Replace("-", "")
                        .ToLowerInvariant()
                        .Equals(Hash);
                }
            }
        }
    }
}