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

using System;
using System.Collections.Generic;
using System.IO;
using SPV3.Compiler.Common;
using SPV3.Domain;
using SPV3.Installer.Domain;
using Directory = SPV3.Domain.Directory;
using File = SPV3.Domain.File;

namespace SPV3.Compiler.Compilers
{
    public class DataCompiler : Common.Compiler
    {
        /// <summary>
        ///     Name for the initial data package. (Meta) + (Core) = 2.
        /// </summary>
        private const int InitialDataPackage = 2;

        /// <inheritdoc />
        public DataCompiler(Compressor compressor, IStatus status = null) : base(compressor, status)
        {
            //
        }

        /// <inheritdoc />
        public override Manifest Compile(Directory source, Directory target)
        {
            Notify("----------------------------");
            Notify("Invoking data compilation...");
            Notify("----------------------------");

            var manifest = new Manifest
            {
                Packages = new List<Package>()
            };

            var infos = new DirectoryInfo(source).GetDirectories();
            var index = InitialDataPackage;

            foreach (var dir in infos)
            {
                var packName = (Name) $"0x{index:X2}.bin";
                var dataFile = (File) Path.Combine(target, packName);

                var infoList = System.IO.Directory.GetFileSystemEntries(dir.FullName, "*");

                var dataPack = new Package
                {
                    Name = packName,
                    Directory = (Directory) dir.Name,
                    Entries = new List<Entry>(),
                    Description = (Description) $"{dir.Name} data"
                };

                foreach (var data in infoList)
                {
                    var type = EntryType.Unknown;

                    if (System.IO.Directory.Exists(data))
                        type = EntryType.Directory;

                    if (System.IO.File.Exists(data))
                        type = EntryType.File;

                    if (type == EntryType.Unknown)
                        throw new FormatException("Cannot infer Entry Type. Does filesystem record exist?");

                    var name = (Name) Path.GetFileName(data);

                    Notify($"Adding new package entry: {dataPack.Name.Value} <= {name.Value}");
                    dataPack.Entries.Add(new Entry
                    {
                        Name = name,
                        Type = type
                    });
                }

                Notify($"Define compression entry: {dataPack.Name.Value} <= {source.Name.Value}");
                var subDir = (Directory) Path.Combine(source, dataPack.Directory);

                Notify($"Compressing package data: {dataPack.Name.Value} <> {dataPack.Description.Value}");
                Compressor.Compress(dataFile, subDir);

                Notify($"Adding entry to manifest: {dataPack.Name.Value} <> {dataPack.Description.Value}");
                manifest.Packages.Add(dataPack);

                index++;
            }

            Notify("----------------------------");
            Notify("Complete data compilation...");
            Notify("----------------------------");

            return manifest;
        }
    }
}