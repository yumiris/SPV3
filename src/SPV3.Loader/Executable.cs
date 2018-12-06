using System.IO;

namespace SPV3.Loader
{
    /// <summary>
    ///     Representation of a valid HCE executable.
    /// </summary>
    public class Executable
    {
        /// <summary>
        ///     Official name of the HCE executable.
        /// </summary>
        public const string Name = "haloce.exe";

        /// <summary>
        ///     Executable constructor.
        /// </summary>
        /// <param name="path">
        ///     <see cref="Path" />
        /// </param>
        public Executable(string path)
        {
            Path = path;
        }

        /// <summary>
        ///     Absolute path to the HCE executable.
        /// </summary>
        public string Path { get; }

        /// <summary>
        ///     Compares the executable on the filesystem against the specifications of a valid HCE executable.
        /// </summary>
        /// <returns>
        ///     True on the executable matching a valid HCE executable; otherwise false.
        /// </returns>
        public bool Verify()
        {
            if (!File.Exists(Path))
                throw new FileNotFoundException("Cannot verify non-existent HCE executable file.");

            return new FileInfo(Path).Length == 0x24B000;
        }
    }
}