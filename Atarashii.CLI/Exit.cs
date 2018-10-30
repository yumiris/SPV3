using System;

namespace Atarashii.CLI
{
    public static class Exit
    {
        /// <summary>
        ///     Writes the inbound error message to STDERR, and ends the application process with a given exit code.
        /// </summary>
        /// <param name="error">
        ///     Error to write to to STDERR.
        /// </param>
        /// <param name="code">
        ///     Exit code to use.
        /// </param>
        public static void WithError(string error, int code)
        {
            new CliOutput().Write(Output.Type.Error, $"Exit code: {code}", error);
            Environment.Exit(code);
        }
    }
}