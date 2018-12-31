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

using System.IO.Compression;
using System.Linq;
using SPV3.Domain;
using SPV3.Installer.Domain;

namespace SPV3.Installer.Installers
{
    /// <inheritdoc />
    public class CoreInstaller : Common.Installer
    {
        /// <summary>
        ///     Name for the core package.
        /// </summary>
        private const string CorePackage = "0x01.bin";

        /// <inheritdoc />
        public CoreInstaller(Directory target, Directory backup, IStatus status = null) : base(target, backup, status)
        {
            //
        }

        /// <inheritdoc />
        public override void Install(Manifest manifest)
        {
            Notify("----------------------------");
            Notify("Invoked core installation...");
            Notify("----------------------------");

            Notify("Running pre-install tasks...");
            Migrate(manifest.Packages.Single(package => package.Name == CorePackage));

            Notify("Installing the core files...");
            ZipFile.ExtractToDirectory(CorePackage, Target);

            Notify("----------------------------");
            Notify("Resolve core installation...");
            Notify("----------------------------");
        }
    }
}