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

namespace SPV3.Installer.Common
{
    /// <summary>
    ///     Extracts Packages defined in the provided Manifest to the provided target Directory.
    /// </summary>
    public abstract class Installer
    {
        /// <summary>
        ///     Directory used for backing up any existing Package Entries.
        /// </summary>
        protected readonly Directory Backup;

        /// <summary>
        ///     Status implementer used for appending installation progress.
        /// </summary>
        protected readonly IStatus Status;

        /// <summary>
        ///     Target directory used for installing the Packages' data.
        /// </summary>
        protected readonly Directory Target;

        /// <summary>
        ///     Installer constructor.
        /// </summary>
        /// <param name="target">
        ///     Target directory used for installing the Packages' data.
        /// </param>
        /// <param name="backup">
        ///     Directory used for backing up any existing Package Entries.
        /// </param>
        /// <param name="status">
        ///     Status implementer used for appending installation progress.
        /// </param>
        protected Installer(Directory target, Directory backup, IStatus status = null)
        {
            Target = target;
            Backup = backup;
            Status = status;
        }

        /// <summary>
        ///     Installs the Packages' data (defined in the Manifest) to the Target directory.
        ///     Any existing Package Entries on the filesystem will be backed up in in a Target directory subdirectory.
        /// </summary>
        /// <param name="manifest">
        ///     Manifest containing information for the Packages to install.
        /// </param>
        public abstract void Install(Manifest manifest);

        /// <summary>
        ///     Backs up the Entries for the provided Package, if they exist on the filesystem. Backing up is done by
        ///     moving (migrating) the data to the specified backup directory.
        /// </summary>
        /// <param name="package">
        ///     Package to backup the entries for.
        /// </param>
        protected void Migrate(Package package)
        {
            if (!System.IO.Directory.Exists(Backup))
                System.IO.Directory.CreateDirectory(Backup);

            /**
             * Each Package from the Manifest is extracted to the Target directory. To handle circumstances where
             * the end-user attempts to install to a directory that already contains SPV3 (grr...), we check for any
             * Entries (Package/archive files) that exist on the filesystem, and back them up before extracting the
             * Package to the filesystem.
             *
             * The alternative would be to delete any existing files, though it's preferable to avoid destructive
             * approaches like these. This is also an entire workaround for the ZipFile.ExtractToDirectory method's
             * inability to overwrite files.
             *
             * Reliance on Package Entries is an alternative to parsing the archive for a list of files, which can
             * get murky if the archive type (currently DEFLATE) is to be changed to another one.
             */

            /**
             * Packages may represent a subdirectory or contain files directly. We infer the subdirectory and append it
             * to the target installation's path, if the package indeed represents a subdirectory.
             */
            string parentSubDirectory, backupSubDirectory;

            if (package.Directory == null)
            {
                parentSubDirectory = Target;
                backupSubDirectory = Backup;
            }
            else
            {
                parentSubDirectory = Path.Combine(Target, package.Directory.Name);
                backupSubDirectory = Path.Combine(Backup, package.Directory.Name);
            }

            foreach (var entry in package.Entries)
            {
                /**
                 * If a Package represents a subdirectory, then the Entries -- on the filesystem -- are actually in the
                 * said subdirectory. Both the Directory & File Move() methods effectively rename files; hence, we must
                 * declare the full paths for the source name (current file/dir) and target name (moved file/dir).
                 */
                var sourceEntry = Path.Combine(parentSubDirectory, entry);
                var targetEntry = Path.Combine(backupSubDirectory, entry);

                if (!File.Exists(sourceEntry) && !System.IO.Directory.Exists(sourceEntry))
                    continue;

                if (!System.IO.Directory.Exists(backupSubDirectory))
                    System.IO.Directory.CreateDirectory(backupSubDirectory);

                /**
                 * Depending on the Entry Type, we treat it as either a file or a directory when moving it. 
                 */
                Notify($"Migrating: {(string) package.Name} :: {(string) entry.Name}");

                if (entry.Type == EntryType.File)
                    File.Move(sourceEntry, targetEntry);

                if (entry.Type == EntryType.Directory)
                    System.IO.Directory.Move(sourceEntry, targetEntry);
            }
        }

        /// <summary>
        ///     Wrapper for IStatus .CommitStatus().
        /// </summary>
        /// <param name="text">
        ///     Text to commit to the IStatus instance.
        /// </param>
        protected void Notify(string text)
        {
            Status?.CommitStatus(text);
        }
    }
}
