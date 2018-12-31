/**
 * Copyright (c) 2018 Emilian Roman
 * 
 * This software is provided 'as-is', without any express or implied
 * warranty. In no event will the authors be held liable for any damages
 * arising from the use of this software.
 * 
 * Permission is granted to anyone to use this software for any purpose,
 * including commercial applications, and to alter it and redistribute it
 * freely, subject to the following restrictions:
 * 
 * 1. The origin of this software must not be misrepresented; you must not
 *    claim that you wrote the original software. If you use this software
 *    in a product, an acknowledgment in the product documentation would be
 *    appreciated but is not required.
 * 2. Altered source versions must be plainly marked as such, and must not be
 *    misrepresented as being the original software.
 * 3. This notice may not be removed or altered from any source distribution.
 */

using System.Collections.Generic;
using System.IO;
using SPV3.Compiler.Common;
using SPV3.Domain;
using SPV3.Installer.Domain;
using Directory = SPV3.Domain.Directory;
using File = SPV3.Domain.File;

namespace SPV3.Compiler.Compilers
{
    public class CoreCompiler : Common.Compiler
    {
        /// <summary>
        ///     Name for the core package.
        /// </summary>
        private const string CorePackage = Prefix + "01" + Suffix;

        /// <inheritdoc />
        public CoreCompiler(Compressor compressor, IStatus status = null) : base(compressor, status)
        {
            //
        }

        /// <inheritdoc />
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
            Compressor.Compress(coreFile, source, fileList);

            Notify($"Adding entry to manifest: {corePack.Name.Value} <> {corePack.Description.Value}");
            manifest.Packages.Add(corePack);

            Notify("----------------------------");
            Notify("Complete core compilation...");
            Notify("----------------------------");

            return manifest;
        }
    }
}