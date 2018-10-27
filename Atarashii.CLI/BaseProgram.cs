using System;

namespace Atarashii.CLI
{
    /// <summary>
    ///     Abstract with shared methods for CLI Programs.
    /// </summary>
    public abstract class BaseProgram
    {
        /// <summary>
        ///     CLI-friendly ASCII art banner.
        /// </summary>
        protected static string Banner => @"
        _                      _     _ _ 
   __ _| |_ __ _ _ __ __ _ ___| |__ (_|_)
  / _` | __/ _` | '__/ _` / __| '_ \| | |
 | (_| | || (_| | | | (_| \__ \ | | | | |
  \__,_|\__\__,_|_|  \__,_|___/_| |_|_|_|
";

        /// <summary>
        ///     Outputs the banner to the CLI.
        /// </summary>
        protected static void ShowBanner()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(Banner);
        }

        /// <summary>
        ///     Exits the program if the inbound arguments are empty.
        /// </summary>
        /// <param name="args">
        ///     Arguments to check the length of.
        /// </param>
        protected static void ExitIfNilArgs(string[] args)
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
        protected static void ExitWithError(string error, int code)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine(error);
            Environment.Exit(code);
        }
    }
}