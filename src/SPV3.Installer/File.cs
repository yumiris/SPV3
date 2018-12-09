using System.IO;

namespace SPV3.Installer
{
    /// <summary>
    ///     Type representing an SPV3 file (e.g. map, dynamic library, etc.).
    /// </summary>
    public class File
    {
        /// <summary>
        ///     Name of the file on the filesystem.
        /// </summary>
        /// <example>
        ///     0x01.pkg
        /// </example>
        /// <example>
        ///     C:\0x01.pkg
        /// </example>
        public Name Name { get; set; }

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
            return System.IO.File.Exists(Name.Value);
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
            string oldPath = Name.Value;
            var newFile = new File
            {
                Name = new Name
                {
                    Value = Path.Combine(directory.Name.Value, Name.Value)
                }
            };

            if (!Exists())
                throw new FileNotFoundException("Source file does not exist on the filesystem.");

            if (newFile.Exists())
                throw new IOException("File already exists in the target directory.");

            System.IO.File.Move(oldPath, newFile.Name.Value);

            Name = newFile.Name;
        }
    }
}