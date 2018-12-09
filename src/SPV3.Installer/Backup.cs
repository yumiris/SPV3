using System.IO;

namespace SPV3.Installer
{
    /// <summary>
    ///     Conducts backup operations on package files.
    /// </summary>
    public class Backup
    {
        /// <summary>
        ///     Directory used for backing up package files to.
        /// </summary>
        /// <example>
        ///     C:\SPV3.2\data-E36E7FB3
        /// </example>
        public Directory Directory { get; set; }

        /// <summary>
        ///     Backs up the package's files on the filesystem to the specified directory, if the files exist.
        /// </summary>
        /// <remarks>
        ///     This should be used in pre-installation contexts to avoid overwrites or conflicts when extracting the
        ///     package.
        /// </remarks>
        /// <param name="package">
        ///     Instance representing the package whose files should be backed up.
        /// </param>
        /// <exception cref="IOException">
        ///     Backed up copy of of the file already exists in the backup directory.
        ///     This can be mitigated by defining an uniquely generated backup directory name to prevent collisions with
        ///     existing backup directories.
        /// </exception>
        public void CommitOn(Package package)
        {
            Directory.Create();

            /**
             * Creates a directory for the package within the backup directory.
             * The directory's name equates to the package's target directory name.
             *
             * For example, the package containing the map data which would be extracted to the maps folder would have
             * existing maps backed up to <backup directory>\maps folder.
             */
            var packageDirectory = new Directory
            {
                Name = new Name
                {
                    Value = Path.Combine(Directory.Name.Value, package.Name.Value)
                }
            };

            packageDirectory.Create();

            /**
             * For each file in the package, we compute its potential backup path. If the file already exists there, an
             * exception is thrown as the backup procedure would be destructive in nature.
             *
             * If the file does not exist in the package's backup directory, the file gets moved from its original
             * location to the target location.
             *
             * The logic carried out above is conducted only if the file already exists on the filesystem.
             */
            foreach (var file in package.Files)
            {
                if (!file.Exists())
                    continue;

                string potentialPath = Path.Combine(packageDirectory.Name.Value, file.Name.Value);

                if (System.IO.File.Exists(potentialPath))
                    throw new IOException("Backed up copy of of the file already exists in the backup directory.");

                file.MoveTo(Directory);
            }
        }
    }
}