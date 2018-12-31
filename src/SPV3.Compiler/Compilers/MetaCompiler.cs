using System.Collections.Generic;
using System.IO;
using SPV3.Compiler.Common;
using SPV3.Domain;
using SPV3.Installer;
using Directory = SPV3.Domain.Directory;
using File = SPV3.Domain.File;

namespace SPV3.Compiler.Compilers
{
    public class MetaCompiler : Common.Compiler
    {
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

        public MetaCompiler(Compressor compressor, IStatus status = null) : base(compressor, status)
        {
            //
        }

        /// <summary>
        ///     Invokes the Core & Data compilers with the provided Directory & Target, and generates a Manifest in the
        ///     Target directory.
        /// </summary>
        /// <param name="source">
        ///     Directory containing SPV3/HCE data.
        /// </param>
        /// <param name="target">
        ///     Directory for the compiled data packages.
        /// </param>
        /// <returns>
        ///     Combined manifest from the Core & Data compilers.
        /// </returns>
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

            var coreManifest = new CoreCompiler(_compressor, _status).Compile(source, target);
            var dataManifest = new DataCompiler(_compressor, _status).Compile(source, target);

            foreach (var package in coreManifest.Packages)
                mainManifest.Packages.Add(package);

            foreach (var package in dataManifest.Packages)
                mainManifest.Packages.Add(package);

            Notify("----------------------------");
            Notify("Resolving metadata binary...");
            Notify("----------------------------");

            var metaPath = (File) Path.Combine(target, ManifestBin);
            new ManifestRepository(metaPath).Save(mainManifest);

            Notify("============================");
            Notify("Completed compile routine...");
            Notify("============================");

            return mainManifest;
        }
    }
}