using System.Diagnostics;
using System.IO;

namespace Atarashii
{
    /// <summary>
    ///     Securely loads the HCE executable.
    /// </summary>
    public class Loader
    {
        /// <summary>
        ///     Executes the HCE executable provided at instantiation.
        /// </summary>
        /// <param name="executable">
        ///     Path to the HCE executable.
        /// </param>
        /// <exception cref="LoaderException">
        ///     Executable is not found.
        /// </exception>
        public void Execute(string executable)
        {
            if (!File.Exists(executable))
                throw new LoaderException($"The specified executable '{executable}' was not found.");

            Process.Start(executable);
        }
    }
}