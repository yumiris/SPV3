/**
 * Copyright (C) 2019 Emilian Roman
 * 
 * This file is part of SPV3.Installer.
 * 
 * SPV3.Installer is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * SPV3.Installer is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with SPV3.Installer.  If not, see <http://www.gnu.org/licenses/>.
 */

using System.Collections.Generic;
using System.IO;
using SPV3.Compiler.Common;
using SPV3.Domain;
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
                    Type = EntryType.File,
                    Size = (Size) new FileInfo(Path.Combine(source, name)).Length
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
