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
using System.IO.Compression;
using System.Linq;
using SPV3.Domain;
using Directory = SPV3.Domain.Directory;

namespace SPV3.Installer.Installers
{
    /// <inheritdoc />
    public class DataInstaller : Common.Installer
    {
        /// <summary>
        ///     Name for the initial data package. (Meta) + (Core) = 2.
        /// </summary>
        private const int InitialDataPackage = 2;

        /// <inheritdoc />
        public DataInstaller(Directory target, Directory backup, IStatus status = null) : base(target, backup, status)
        {
            //
        }

        /// <inheritdoc />
        public override void Install(Manifest manifest)
        {
            Notify("----------------------------");
            Notify("Invoked core installation...");
            Notify("----------------------------");

            foreach (var package in manifest.Packages.Where(package => package.Name != "0x01.bin"))
            {
                Notify("Migrating existing for pack:" + $"{package.Name} => {package.Directory}");
                Migrate(package);

                Notify("Installing data from package:" + $"{package.Name} => {package.Directory}");
                var target = Path.Combine(Target, package.Directory);
                ZipFile.ExtractToDirectory(package.Name, target);
            }

            Notify("----------------------------");
            Notify("Resolve data installation...");
            Notify("----------------------------");
        }
    }
}
