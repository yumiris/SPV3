using System.Collections.Generic;
using System.IO;
using System.Linq;
using SPV3.Domain;
using SPV3.Installer;
using Directory = SPV3.Domain.Directory;
using File = SPV3.Domain.File;

namespace SPV3.Compiler
{
    /// <summary>
    ///     Compiles SPV3 source to redistributable packages w/ manifest.
    /// </summary>
    public class Compiler
    {
        private const string ManifestBin = "0x00.bin";
        private const string CorePackage = "0x01.bin";

        private static readonly string SevenZipPath = Path.Combine("7z", "7za.exe");
        private static readonly string SevenZipHash = "e93c0c45d84f328afba850e87ac7398c";

        private static readonly Version Version = new Version
        {
            Major = 1,
            Minor = 0,
            Patch = 0
        };

        private readonly List<Package> _packages = new List<Package>();

        private readonly Directory _source;
        private readonly Directory _target;

        public Compiler(Directory source, Directory target)
        {
            _source = source;
            _target = target;
        }

        public void Compile()
        {
            CreateCorePackage();
            CreateDirPackages();
            CreateMetadataBin();
        }

        private void CreateMetadataBin()
        {
            var metaPath = (File) Path.Combine(_target, ManifestBin);

            var metadata = new Metadata
            {
                Version = Version,
                Packages = _packages
            };

            new MetadataRepository(metaPath).Save(metadata);
        }

        private void CreateCorePackage()
        {
            var core = new DirectoryInfo(_source)
                .GetFiles("*.*");

            void Invoke()
            {
                var target = (File) Path.Combine(_target, CorePackage);

                var files = core
                    .Select(info => (File) info.Name)
                    .ToList();

                new Compressor().Compress(target, _source, files);
            }

            void Append()
            {
                var files = core
                    .Select(file => (Entry) file.Name)
                    .ToList();

                var package = new Package
                {
                    Name = (Name) "0x01.bin",
                    Entries = files
                };

                _packages.Add(package);
            }

            Invoke();
            Append();
        }

        private void CreateDirPackages()
        {
            void Invoke(string name, FileSystemInfo directory)
            {
                var target = (File) Path.Combine(_target, $"{name}");
                var source = (Directory) Path.Combine(_source, directory.Name);

                new Compressor().Compress(target, source);
            }

            void Append(string name, FileSystemInfo directory)
            {
                var files = System.IO.Directory
                    .GetFileSystemEntries(directory.FullName, "*")
                    .Cast<Entry>()
                    .ToList();

                _packages.Add(new Package
                {
                    Name = (Name) name,
                    Entries = files
                });
            }

            var index = 2;

            foreach (var directory in new DirectoryInfo(_source).GetDirectories())
            {
                var name = $"{index:D2}.bin";

                Invoke(name, directory);
                Append(name, directory);

                index++;
            }
        }
    }
}