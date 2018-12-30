using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using SPV3.Domain;
using Directory = SPV3.Domain.Directory;
using File = System.IO.File;

namespace SPV3.Installer
{
    /// <summary>
    ///     Extracts Packages defined in the provided Manifest to the provided target Directory.
    /// </summary>
    public class Installer
    {
        /// <summary>
        ///     Directory used for backing up any existing Package Entries.
        /// </summary>
        private readonly Directory _backup;

        /// <summary>
        ///     Manifest used by the installer to decide what Packages should be installed.
        /// </summary>
        private readonly Manifest _manifest;

        /// <summary>
        ///     Status implementer used for appending installation progress.
        /// </summary>
        private readonly IStatus _status;

        /// <summary>
        ///     Target directory used for installing the Packages' data.
        /// </summary>
        private readonly Directory _target;

        /// <summary>
        ///     Installer constructor.
        /// </summary>
        /// <param name="manifest">Manifest used by the installer to decide what Packages should be installed.</param>
        /// <param name="target">Target directory used for installing the Packages' data.</param>
        /// <param name="status">Status implementer used for appending installation progress.</param>
        public Installer(Manifest manifest, Directory target, IStatus status = null)
        {
            _manifest = manifest;

            _target = target;
            _status = status;

            _backup = (Directory) Path.Combine(_target, "SPV3-" + Guid.NewGuid());
        }

        /// <summary>
        ///     Installs the Packages' data (defined in the Manifest) to the Target directory.
        ///     Any existing Package Entries on the filesystem will be backed up in in a Target directory subdirectory.
        /// </summary>
        public void Install()
        {
            /**
             * Creates the backup Directory on the filesystem.
             */
            void Initiate()
            {
                Notify("============================");
                Notify("Initiated install routine...");
                Notify("============================");

                if (!System.IO.Directory.Exists(_backup))
                    System.IO.Directory.CreateDirectory(_backup);
            }

            /**
             * Deletes the backup Directory on the filesystem if it's empty.
             */
            void Complete()
            {
                if (!System.IO.Directory.EnumerateFileSystemEntries(_backup).Any())
                    System.IO.Directory.Delete(_backup);

                Notify("============================");
                Notify("Completed install routine...");
                Notify("============================");
            }

            /**
             * Extracts the data for each Package on the filesystem to the target Directory.
             */
            void Extract(Package package)
            {
                Notify("----------------------------------------------------------------------");
                Notify($"Installing: {(string) package.Name} :: {(string) package.Description}");
                Notify("----------------------------------------------------------------------");

                /**
                 * If the package represents a subdirectory, then the target should be a subdirectory created within the
                 * installation target directory.
                 */
                string target;
                if (package.Directory.Name == null)
                {
                    target = _target;
                }
                else
                {
                    target = Path.Combine(_target, package.Directory.Name);

                    if (!System.IO.Directory.Exists(target))
                        System.IO.Directory.CreateDirectory(target);
                }
                
                /**
                 * Install the data to the final target.
                 */
                ZipFile.ExtractToDirectory(package.Name, target);

                foreach (var entry in package.Entries)
                    Notify($"Installed: {(string) package.Name} :: {(string) entry.Name}");
            }

            Initiate();

            foreach (var package in _manifest.Packages)
            {
                Migrate(package);
                Extract(package);
            }

            Complete();
        }

        /// <summary>
        ///     Backs up the Entries for the provided Package, if they exist on the filesystem.
        /// </summary>
        /// <param name="package">
        ///     Package to backup the entries for.
        /// </param>
        private void Migrate(Package package)
        {
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

            Notify("----------------------------------------------------------------------");
            Notify($"Backing up: {(string) package.Name} :: {(string) package.Description}");
            Notify("----------------------------------------------------------------------");

            /**
             * Packages may represent a subdirectory or contain files directly. We infer the subdirectory and append it
             * to the target installation's path, if the package indeed represents a subdirectory.
             */
            string parentSubDirectory, backupSubDirectory;

            if (package.Directory.Name == null)
            {
                parentSubDirectory = _target;
                backupSubDirectory = _backup;
            }
            else
            {
                parentSubDirectory = Path.Combine(_target, package.Directory.Name);
                backupSubDirectory = Path.Combine(_backup, package.Directory.Name);
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
                 * If the Entry (within the potential subdirectory) is a file, then we must invoke the File.Move method
                 * to move the file from the source to the target location.
                 */
                if (entry.Type == EntryType.File)
                    File.Move(sourceEntry, targetEntry);

                /**
                 * Same principle as above; however, for directories.
                 */
                if (entry.Type == EntryType.Directory)
                    System.IO.Directory.Move(sourceEntry, targetEntry);

                Notify($"Migrating: {(string) package.Name} :: {(string) entry.Name}");
            }
        }

        /// <summary>
        ///     Wrapper for IStatus .CommitStatus().
        /// </summary>
        /// <param name="text">
        ///     Text to commit to the IStatus instance.
        /// </param>
        private void Notify(string text)
        {
            _status?.CommitStatus(text);
        }
    }
}