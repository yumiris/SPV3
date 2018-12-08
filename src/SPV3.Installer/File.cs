using System.IO;

namespace SPV3.Installer
{
    /// <summary>
    ///     Type representing an SPV3 file (e.g. map, dynamic library, etc.).
    /// </summary>
    public class File
    {
        /// <summary>
        ///     Name of the file.
        /// </summary>
        /// <example>
        ///     0x01.pkg
        /// </example>
        public Name Name { get; set; }

        /// <summary>
        ///     Absolute path of the file.
        /// </summary>
        /// <example>
        ///     C:\0x01.pkg
        /// </example>
        public Path Path { get; set; }

        /// <summary>
        ///     Description of the file.
        /// </summary>
        /// <example>
        ///     SPV3.2 Core Installation Data
        /// </example>
        public Description Description { get; set; }

        /// <summary>
        ///     Checks if the file exists on the filesystem using the Path value.
        /// </summary>
        /// <returns>
        ///     True if file exists, otherwise false.
        /// </returns>
        public bool Exists()
        {
            return System.IO.File.Exists(Path.Value);
        }


        /// <summary>
        ///     Moves the file on the filesystem to the new path, and updates the Path value.
        /// </summary>
        /// <param name="directory">
        ///     Instance representing the directory to move the file to.
        /// </param>
        /// <exception cref="FileNotFoundException">
        ///     Source file does not exist on the filesystem.
        /// </exception>
        /// <exception cref="IOException">
        ///     File already exists in the target directory.
        /// </exception>
        public void MoveTo(Directory directory)
        {
            string oldPath = Path.Value;
            var newFile = new File
            {
                Name = Name,
                Path = new Path
                {
                    Value = System.IO.Path.Combine(directory.Path.Value, Name.Value)
                }
            };

            if (!Exists())
                throw new FileNotFoundException("Source file does not exist on the filesystem.");

            if (newFile.Exists())
                throw new IOException("File already exists in the target directory.");

            System.IO.File.Move(oldPath, newFile.Path.Value);

            Path = newFile.Path;
        }
    }
}