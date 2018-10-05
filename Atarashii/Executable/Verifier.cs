using System.IO;

namespace Atarashii.Executable
{
    /// <summary>
    ///     Verifies the validity of the specified HCE executable.
    /// </summary>
    public class Verifier
    {
        /// <summary>
        ///     Known file length of the 1.10 executable.
        /// </summary>
        private const int ValidLength = 0x24B000;

        /// <summary>
        ///     Checks if the specified HCE executable is valid.
        /// </summary>
        /// <param name="executable">
        ///     HCE executable to verify.
        /// </param>
        /// <returns>
        ///     True on executable being deemed as valid; otherwise false.
        /// </returns>
        /// <exception cref="VerifierException">
        ///     Executable is not found.
        /// </exception>
        public bool Verify(string executable)
        {
            if (!File.Exists(executable))
                throw new VerifierException($"The specified executable '{executable}' was not found.");

            return new FileInfo(executable).Length == ValidLength;
        }
    }
}