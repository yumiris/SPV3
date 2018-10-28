using System;
using System.Diagnostics;
using System.IO;

namespace Atarashii.Loader
{
    public class Executable : IVerifiable
    {
        /// <summary>
        ///     HCE executable name.
        /// </summary>
        public const string Name = "haloce.exe";

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

        /// <inheritdoc />
        /// <returns>
        ///     False if:
        ///     - Path does not exist.
        ///     - Executable is invalid.
        /// </returns>
        public Verification Verify()
        {
            if (!File.Exists(Path))
                return new Verification(false, "The specified executable was not found.");

            if (new FileInfo(Path).Length != ValidLength)
                return new Verification(false, "The specified executable is invalid in size.");

            return new Verification(true);
        }

        /// <summary>
        ///     Executes the given HCE executable.
        /// </summary>
        /// <param name="verify">
        ///     Verify the HCE executable.
        /// </param>
        /// <exception cref="LoaderException">
        ///     The specified executable was not found.
        ///     - or -
        ///     The specified executable is deemed invalid.
        ///     - or -
        ///     Could not infer executable name from the path.
        ///     - or -
        ///     Could not infer working directory from the path.
        /// </exception>
        public void Load(bool verify = true)
        {
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
                    FileName = System.IO.Path.GetFileName(Path) ??
                               throw new LoaderException("Could not infer executable name from the path."),
                    WorkingDirectory = System.IO.Path.GetDirectoryName(Path) ??
                                       throw new LoaderException("Could not infer working directory from the path.")
                }
            }.Start();
        }
    }
}