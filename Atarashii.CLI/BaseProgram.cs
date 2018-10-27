using System;

namespace Atarashii.CLI
{
    /// <summary>
    ///     Abstract for CLI Programs with shared methods.
    /// </summary>
    public abstract class BaseProgram
    {
        /// <summary>
        ///     Exits the program if the inbound arguments are empty.
        /// </summary>
        /// <param name="args">
        ///     Arguments to check the length of.
        /// </param>
        public static void ExitIfNilArgs(string[] args)
        {
            if (args.Length == 0) ExitWithError("No arguments provided.", 1);
        }

        /// <summary>
        ///     Writes the inbound error message to STDERR, and ends the application process with a given exit code.
        /// </summary>
        /// <param name="error">
        ///     Error to write to to STDERR.
        /// </param>
        /// <param name="code">
        ///     Exit code to use.
        /// </param>
        public static void ExitWithError(string error, int code)
        {
            Console.Error.WriteLine(error);
            Environment.Exit(code);
        }
    }
}