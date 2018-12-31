using System.Collections.Generic;
using System.IO;
using SPV3.Domain;
using SPV3.Installer;
using Directory = SPV3.Domain.Directory;
using File = SPV3.Domain.File;

namespace SPV3.Compiler
{
    public class CoreCompiler : Compiler
    {
        /// <summary>
        ///     Name for the core package.
        /// </summary>
        private const string CorePackage = Prefix + "01" + Suffix;

        public CoreCompiler(Compressor compressor, IStatus status = null) : base(compressor, status)
        {
            //
        }

        public override Manifest Compile(Directory source, Directory target)
        {
            Notify("----------------------------");
            Notify("Invoking core compilation...");
            Notify("----------------------------");

            var manifest = new Manifest
            {
                Packages = new List<Package>()
            };

            var coreFile = (File) Path.Combine(target, CorePackage);

            var infoList = new DirectoryInfo(source).GetFiles("*.*");
            var fileList = new List<File>();

            var corePack = new Package
            {
                Name = (Name) CorePackage,
                Description = (Description) "SPV3/HCE Core data",
                Entries = new List<Entry>()
            };

            foreach (var file in infoList)
            {
                var name = (Name) Path.GetFileName(file.Name);

                Notify($"Adding compression entry: {corePack.Name.Value} <= {name.Value}");
                fileList.Add(new File
                {
                    Name = name
                });

                Notify($"Adding new package entry: {corePack.Name.Value} <= {name.Value}");
                corePack.Entries.Add(new Entry
                {
                    Name = name,
                    Type = EntryType.File
                });
            }

            Notify($"Compressing package data: {corePack.Name.Value} <> {corePack.Description.Value}");
            _compressor.Compress(coreFile, source, fileList);

            Notify($"Adding entry to manifest: {corePack.Name.Value} <> {corePack.Description.Value}");
            manifest.Packages.Add(corePack);

            Notify("----------------------------");
            Notify("Complete core compilation...");
            Notify("----------------------------");

            return manifest;
        }
    }
}