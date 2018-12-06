using System;

namespace SPV3.Loader
{
    /// <summary>
    ///     Representation of a valid HCE executable.
    /// </summary>
    public class Executable
    {
        /// <summary>
        ///     Executable constructor.
        /// </summary>
        /// <param name="path">
        ///     <see cref="Path" />
        /// </param>
        public Executable(string path, ExecutableParameters parameters)
        {
            Path = path;
        }

        /// <summary>
        ///     Absolute path to the HCE executable.
        /// </summary>
        public string Path { get; }

        /// <summary>
        ///     Parameters used for initialising the HCE executable process.
        /// </summary>
        public ExecutableParameters Parameters { get; set; }

        /// <summary>
        ///     Compares the executable on the filesystem against the specifications of a valid HCE executable.
        /// </summary>
        /// <returns>
        ///     True on the executable matching a valid HCE executable; otherwise false.
        /// </returns>
        public bool Verify()
        {
            throw new NotImplementedException();
        }
    }
}