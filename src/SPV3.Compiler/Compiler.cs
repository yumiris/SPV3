using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using SPV3.Domain;
using SPV3.Installer;
using Directory = SPV3.Domain.Directory;
using File = SPV3.Domain.File;
using Version = SPV3.Domain.Version;

namespace SPV3.Compiler
{
    /// <summary>
    ///     Compiles SPV3 source to redistributable packages w/ manifest.
    /// </summary>
    public class Compiler
    {
        /// <summary>
        ///     Prefix used for the package/manifest files.
        /// </summary>
        private const string Prefix = "0x";

        /// <summary>
        ///     Suffix (or extension) used for the package/manifest files.
        /// </summary>
        private const string Suffix = ".bin";

        /// <summary>
        ///     Name for the core package.
        /// </summary>
        private const string CorePackage = Prefix + "01" + Suffix;

        /// <summary>
        ///     Name for the initial data package. (Meta) + (Core) = 2.
        /// </summary>
        private const int InitialDataPackage = 2;

        /// <summary>
        ///     Name for the manifest file.
        /// </summary>
        private static readonly string ManifestBin = Manifest.Name;

        /// <summary>
        ///     Current manifest/compiler version.
        /// </summary>
        private static readonly Version Version = new Version
        {
            Major = 1,
            Minor = 0,
            Patch = 0
        };

        /// <summary>
        ///     Compressor used for the package deflation.
        /// </summary>
        private readonly Compressor _compressor;

        /// <summary>
        ///     Created & compiled packages.
        /// </summary>
        private readonly List<Package> _packages = new List<Package>();

        /// <summary>
        ///     SPV3 source data directory.
        /// </summary>
        private readonly Directory _source;

        /// <summary>
        ///     Status implementer used for appending compilation progress.
        /// </summary>
        private readonly IStatus _status;

        /// <summary>
        ///     Target directory for manifest & packages.
        /// </summary>
        private readonly Directory _target;

        /// <summary>
        ///     Compiler constructor.
        /// </summary>
        /// <param name="source">
        ///     SPV3 source data directory.
        /// </param>
        /// <param name="target">
        ///     Target directory for manifest & packages.
        /// </param>
        /// <param name="compressor">
        ///     Compressor used for the package deflation.
        /// </param>
        /// <param name="status">
        ///     Status implementer used for appending compilation progress.
        /// </param>
        public Compiler(Directory source, Directory target, Compressor compressor, IStatus status = null)
        {
            _source = source;
            _target = target;
            _compressor = compressor;
            _status = status;
        }

        /// <summary>
        ///     Compiles the provided source directory into DEFLATE packages & manifest in the target directory.
        /// </summary>
        public void Compile()
        {
            Notify("============================");
            Notify("Initiated compile routine...");
            Notify("============================");

            CreateCorePackage();
            CreateDirPackages();
            CreateMetadataBin();

            Notify("============================");
            Notify("Completed compile routine...");
            Notify("============================");
        }

        /// <summary>
        ///     Creates the metadata binary in the target directory.
        /// </summary>
        private void CreateMetadataBin()
        {
            var metaPath = (File) Path.Combine(_target, ManifestBin);

            var metadata = new Manifest
            {
                Version = Version,
                Packages = _packages
            };

            Notify("----------------------------");
            Notify("Resolving metadata binary...");
            Notify("----------------------------");

            new ManifestRepository(metaPath).Save(metadata);
        }

        /// <summary>
        ///     Creates the core DEFLATE package and adds it as a Package in the Packages list.
        ///     The package files & Entries are all of the root files in the source directory.
        /// </summary>
        private void CreateCorePackage()
        {
            Notify("----------------------------");
            Notify("Invoking core compilation...");
            Notify("----------------------------");

            var package = new Package
            {
                Name = (Name) CorePackage,
                Description = (Description) "Core SPV3/HCE data"
            };

            CompressPackage(_source, (File) Path.Combine(_target, package.Name),
                new DirectoryInfo(_source)
                    .GetFiles("*.*"));

            AddPackageFiles(package, _source);
        }

        /// <summary>
        ///     Creates the DEFLATE packages for subdirectories in the source folder, and adds each subdirectory as a
        ///     Package in the Packages list. Each created Package's Entries represent the files in the respective
        ///     subdirectory.
        /// </summary>
        private void CreateDirPackages()
        {
            Notify("----------------------------");
            Notify("Invoking data compilation...");
            Notify("----------------------------");

            var index = InitialDataPackage;
            foreach (var directory in new DirectoryInfo(_source).GetDirectories())
            {
                var name = $"{Prefix}{index:D2}{Suffix}";

                var package = new Package
                {
                    Name = (Name) name,
                    Description = (Description) $"{directory.Name} data"
                };

                CompressPackage((Directory) directory.FullName, (File) Path.Combine(_target, name));
                AddPackageFiles(package, (Directory) directory.FullName);

                index++;
            }
        }

        private void AddPackageFiles(Package package, Directory source)
        {
            Notify($"Generating package metadata: {package} <= {Path.GetDirectoryName(source)}");

            var filesList = System.IO.Directory.GetFileSystemEntries(source, "*");
            var entryList = new List<Entry>();

            foreach (var file in filesList)
            {
                var type = EntryType.Unknown;

                if (System.IO.File.Exists(file))
                    type = EntryType.File;

                if (System.IO.Directory.Exists(file))
                    type = EntryType.Directory;

                if (type == EntryType.Unknown)
                    throw new FormatException("Cannot infer Entry Type. Does filesystem record exist?");

                entryList.Add(new Entry
                {
                    Name = (Name) Path.GetFileName(file),
                    Type = type
                });
            }

            package.Entries = entryList;
        }

        private void CompressPackage(Directory source, File target, FileInfo[] files = null)
        {
            Notify($"Compressing filesystem data: {Path.GetFileName(target)} <= {Path.GetFileName(source)}");

            try
            {
                if (files == null)
                {
                    _compressor.Compress(target, source);
                }
                else
                {
                    _compressor.Compress(target, source, files.Select(file => new File
                    {
                        Name = (Name) file.Name
                    }).ToList());
                }
            }
            catch (SecurityException)
            {
                Notify("*****************************************");
                Notify("External compressor binary hash mismatch.");
                Notify("*****************************************");
            }
            catch (FileNotFoundException exception)
            {
                Notify(exception.Message);
            }
        }

        /// <summary>
        ///     Wrapper for IStatus .CommitStatus().
        /// </summary>
        /// <param name="text">
        ///     Text to commit to the IStatus instance.
        /// </param>
        private void Notify(string text)
        {
            _status?.CommitStatus(text);
        }
    }
}