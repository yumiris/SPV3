using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private const string SevenZipFile = "7za.exe";

        /// <summary>
        ///     Path on the filesystem containing the 7-Zip 18.05 (2018-04-30) executable & DLLs.
        ///     This folder is distributed with the SPV3 Compiler.
        /// </summary>
        private const string SevenZipPath = "7z";

        /// <summary>
        ///     MD5 hash of the 7-Zip 18.05 (2018-04-30) executable.
        /// </summary>
        private const string SevenZipHash = "fde874e8d442e3f0469b3d2f86a45739";

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
        private string Path { get; }

        /// <summary>
        ///     Hash of the compressor executable for pre-execution verification.
        /// </summary>
        private string Hash { get; }

        /// <summary>
        ///     Packs the directories and files in the provided source to the provided target.
        /// </summary>
        /// <param name="source">
        ///     Source to pack. In the context of compiling SPV3, the provided path should represent a directory that
        ///     contains the SPV3 maps and HCE core files.
        /// </param>
        /// <param name="target">
        ///     Target to create the packages in. In the context of compiling SPV3, the provided path should represent
        ///     an empty directory that will then be distributed as an ISO or ZIP package.
        /// </param>
        /// <exception cref="DirectoryNotFoundException">
        ///     Source directory does not exist.
        ///     - or -
        ///     Target directory does not exist.
        /// </exception>
        /// <exception cref="SecurityException">
        ///     Compression executable hash does not match the expected one.
        /// </exception>
        public void Pack(string source, string target)
        {
            if (!Directory.Exists(source))
                throw new DirectoryNotFoundException("Source directory does not exist.");

            if (!Directory.Exists(source))
                throw new DirectoryNotFoundException("Target directory does not exist.");

            if (!Verify())
                throw new SecurityException("Compression executable hash does not match the expected one.");

            var sourceDirectory = new DirectoryInfo(source);

            /**
             * The root files in the source folder will all be packed into a single core package.
             * Root files include the HCE executable, OpenSauce & core libraries, configurations, etc.
             */
            CompressFiles("core.pkg", sourceDirectory.GetFiles("*.*"), target);

            /**
             * Each directory in the provided source will be packed into an individual package.
             * It is expected that each discovered directory represents a group of SPV3/HCE-related files.
             * Example folders include maps, redist, shaders, watson, etc.
             */
            CompressDirectories(sourceDirectory.GetDirectories(), target);
        }

        /// <summary>
        ///     Verifies the hash of the compression executable on the filesystem against the hash provided in the
        ///     constructor.
        /// </summary>
        /// <returns>
        ///     True if the computed hashes match, otherwise false.
        /// </returns>
        /// <exception cref="FileNotFoundException">
        ///     Compression executable does not exist.
        /// </exception>
        private bool Verify()
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

        /// <summary>
        ///     Compresses the provided files into a package at the specified target.
        /// </summary>
        /// <param name="package">
        ///     Package name to compile all of the files to.
        ///     <example>
        ///         core.pkg
        ///     </example>
        /// </param>
        /// <param name="files">
        ///     Files to compile into the package.
        ///     <example>
        ///         {
        ///         haloce.exe,
        ///         dsound.dll,
        ///         config.txt,
        ///         dinput8.dll
        ///         }
        ///     </example>
        /// </param>
        /// <param name="target">
        ///     Target to create the packages in. In the context of compiling SPV3, the provided path should represent
        ///     an empty directory that will then be distributed as an ISO or ZIP package.
        /// </param>
        private void CompressFiles(string package, IEnumerable<FileInfo> files, string target)
        {
            foreach (var file in files)
            {
                var pack = System.IO.Path.Combine(target, package);
                var args = $"a -tzip {pack} {file.Name}";
                InvokeProcess(args);
            }
        }

        /// <summary>
        ///     Compresses each provided directory into a package in the specified target.
        /// </summary>
        /// <param name="directories">
        ///     Directories to create packages for.
        ///     <example>
        ///         {
        ///         C:\SPV3\maps,
        ///         C:\SPV3\redist,
        ///         C:\SPV3\shaders,
        ///         }
        ///     </example>
        /// </param>
        /// <param name="target">
        ///     Target to create the packages in. In the context of compiling SPV3, the provided path should represent
        ///     an empty directory that will then be distributed as an ISO or ZIP package.
        /// </param>
        private void CompressDirectories(IEnumerable<DirectoryInfo> directories, string target)
        {
            foreach (var directory in directories)
            {
                var pack = System.IO.Path.Combine(target, $"{directory.Name}.pkg");
                var args = $"a -tzip {pack} {directory.Name}";
                InvokeProcess(args);
            }
        }

        /// <summary>
        ///     Invokes the compression process with the specified arguments, and hangs the thread until the process is
        ///     finished.
        /// </summary>
        /// <param name="args">
        ///     Arguments to pass onto the executing process.
        /// </param>
        private void InvokeProcess(string args)
        {
            var workingDirectory = System.IO.Path.GetDirectoryName(Path) ??
                                   throw new FormatException("Could not infer working directory.");
            
            var executableBinary = System.IO.Path.GetFileName(Path) ??
                                   throw new FormatException("Could not infer executable binary.");

            var process = new Process
            {
                StartInfo =
                {
                    FileName = executableBinary,
                    WorkingDirectory = workingDirectory,
                    Arguments = args
                }
            };

            process.Start();
            process.WaitForExit();
        }
    }
}