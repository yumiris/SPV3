using System.Diagnostics;
using System.IO;

namespace Atarashii.Executable
{
    /// <summary>
    ///     Securely loads the HCE executable.
    /// </summary>
    public class Loader
    {
        /// <summary>
        ///     Executes the given HCE executable.
        /// </summary>
        /// <param name="executable">
        ///     Path to the HCE executable.
        /// </param>
        /// <exception cref="LoaderException">
        ///     The specified executable was not found.
        /// </exception>
        public void Execute(string executable)
        {
            if (!File.Exists(executable))
                throw new LoaderException($"The specified executable '{executable}' was not found.");

            new Process
            {
                StartInfo =
                {
                    FileName = executable,
                    WorkingDirectory = Path.GetDirectoryName(executable)
                }
            }.Start();
        }

        /// <summary>
        ///     Executes the given HCE executable if deemed as valid..
        /// </summary>
        /// <param name="executable">
        ///     Executable to verify and load.
        /// </param>
        /// <param name="verifier">
        ///     Verifier to use on the executable.
        /// </param>
        /// <exception cref="LoaderException">
        ///     Executable is either not found or is invalid.
        /// </exception>
        public void Execute(string executable, Verifier verifier)
        {
            if (!File.Exists(executable))
                throw new LoaderException($"The specified executable '{executable}' was not found.");

            if (!verifier.Verify(executable))
                throw new LoaderException($"The specified executable '{executable}' is deemed invalid.");

            Execute(executable);
        }
    }
}