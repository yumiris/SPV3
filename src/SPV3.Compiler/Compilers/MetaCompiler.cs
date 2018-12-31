using System.Collections.Generic;
using System.IO;
using SPV3.Compiler.Common;
using SPV3.Domain;
using SPV3.Installer;
using Directory = SPV3.Domain.Directory;
using File = SPV3.Domain.File;

namespace SPV3.Compiler.Compilers
{
    /// <inheritdoc />
    public class MetaCompiler : Common.Compiler
    {
        /// <summary>
        ///     Current manifest/compiler version.
        /// </summary>
        private static readonly Version Version = new Version
        {
            Major = 1,
            Minor = 0,
            Patch = 0
        };

        /// <inheritdoc />
        public MetaCompiler(Compressor compressor, IStatus status = null) : base(compressor, status)
        {
            //
        }

        /// <inheritdoc />
        public override Manifest Compile(Directory source, Directory target)
        {
            Notify("============================");
            Notify("Initiated compile routine...");
            Notify("============================");

            var mainManifest = new Manifest
            {
                Version = Version,
                Packages = new List<Package>()
            };

            var coreManifest = new CoreCompiler(Compressor, Status).Compile(source, target);
            var dataManifest = new DataCompiler(Compressor, Status).Compile(source, target);

            foreach (var package in coreManifest.Packages)
                mainManifest.Packages.Add(package);

            foreach (var package in dataManifest.Packages)
                mainManifest.Packages.Add(package);

            Notify("----------------------------");
            Notify("Resolving metadata binary...");
            Notify("----------------------------");

            var metaPath = (File) Path.Combine(target, Manifest.Name);
            new ManifestRepository(metaPath).Save(mainManifest);

            Notify("============================");
            Notify("Completed compile routine...");
            Notify("============================");

            return mainManifest;
        }
    }
}