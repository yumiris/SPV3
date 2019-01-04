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

using System.IO;
using SPV3.Domain;
using Directory = SPV3.Domain.Directory;
using File = System.IO.File;

namespace SPV3.Installer.Installers
{
    /// <inheritdoc />
    public class MetaInstaller : Common.Installer
    {
        /// <inheritdoc />
        public MetaInstaller(Directory target, Directory backup, IStatus status = null) : base(target, backup, status)
        {
            //
        }

        /// <inheritdoc />
        public override void Install(Manifest manifest)
        {
            Notify("============================");
            Notify("Initiated install routine...");
            Notify("============================");

            /**
             * Conduct the installations for the core & data.
             */
            new CoreInstaller(Target, Backup, Status).Install(manifest);
            new DataInstaller(Target, Backup, Status).Install(manifest);

            /**
             * Copy the manifest to the target directory; the loader should use it for verifying the maps.
             */
            Notify("Copying 0x00.bin manifest...");
            File.Copy(Manifest.Name, Path.Combine(Target, Manifest.Name));

            Notify("============================");
            Notify("Completed install routine...");
            Notify("============================");
        }
    }
}