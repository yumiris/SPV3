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
                throw new LoaderException("The specified executable was not found.");

            if (verify)
            {
                var state = Verify();
                if (!state.IsValid)
                    throw new LoaderException(state.Reason);
            }

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
        ///     Verification object representing the verification outcome.
        /// </returns>
        public Verification Verify()
        {
            if (!File.Exists(Path))
                return new Verification(false, "The specified executable was not found.");

            if (new FileInfo(Path).Length != ValidLength)
                return new Verification(false, "The specified executable is invalid in size.");

            return new Verification(true);
        }
    }
}