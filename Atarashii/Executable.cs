using System.Diagnostics;
using System.IO;
using Atarashii.Exceptions;

namespace Atarashii
{
    public class Executable
    {
        /// <summary>
        ///     HCE executable name.
        /// </summary>
        public const string Name = @"haloce.exe";

        /// <summary>
        ///     Known file length of the 1.10 executable.
        /// </summary>
        private const int ValidLength = 0x24B000;

        public Executable(string path)
        {
            Path = path;
        }

        /// <summary>
        ///     Executable file path.
        /// </summary>
        public string Path { get; }

        /// <summary>
        ///     Executes the given HCE executable.
        /// </summary>
        /// <param name="verify">
        ///     Verify the HCE executable.
        /// </param>
        /// <exception cref="LoaderException">
        ///     The specified executable was not found.
        /// </exception>
        public void Load(bool verify = true)
        {
            if (!File.Exists(Path))
                throw new LoaderException($"The specified executable '{Path}' was not found.");

            if (verify)
                if (!Verify())
                    throw new LoaderException($"The specified executable '{Path}' is deemed invalid.");

            new Process
            {
                StartInfo =
                {
                    FileName = Path,
                    WorkingDirectory = System.IO.Path.GetDirectoryName(Path)
                }
            }.Start();
        }

        /// <summary>
        ///     Checks if the specified HCE executable is valid.
        /// </summary>
        /// <returns>
        ///     True on executable being deemed as valid; otherwise false.
        /// </returns>
        /// <exception cref="VerifierException">
        ///     Executable is not found.
        /// </exception>
        public bool Verify()
        {
            if (!File.Exists(Path))
                throw new VerifierException($"The specified executable '{Path}' was not found.");

            return new FileInfo(Path).Length == ValidLength;
        }
    }
}