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