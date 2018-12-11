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
            return System.IO.File.Exists(this);
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
            var newFile = (File) Path.Combine(directory, this);

            if (!Exists())
                throw new FileNotFoundException("Source file does not exist on the filesystem.");

            if (newFile.Exists())
                throw new IOException("File already exists in the target directory.");

            System.IO.File.Move(this, newFile);

            Name = newFile.Name;
        }

        /// <summary>
        ///     Implicitly represents object as string.
        /// </summary>
        /// <param name="file">
        ///     Object instance.
        /// </param>
        /// <returns>
        ///     File.Name
        /// </returns>
        public static implicit operator string(File file)
        {
            return file.Name;
        }

        /// <summary>
        ///     Explicitly represents string as object.
        /// </summary>
        /// <param name="value">
        ///     File.Name
        /// </param>
        /// <returns>
        ///     Object instance.
        /// </returns>
        public static explicit operator File(string value)
        {
            return new File
            {
                Name = new Name
                {
                    Value = value
                }
            };
        }
    }
}