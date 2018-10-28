using System;
using System.Collections.Generic;
using Atarashii.CLI.Outputs;

namespace Atarashii.CLI.Common
{
    /// <summary>
    ///     Abstract type representing an invokable Atarashii CLI command.
    /// </summary>
    public abstract class Command
    {
        /// <summary>
        ///     Available invokable commands.
        /// </summary>
        protected static readonly List<string> Available = new List<string>
        {
            "loader",
            "opensauce",
            "profile"
        };

        protected static class Exit
        {
            /// <summary>
            ///     Exits the program if the inbound arguments are empty.
            /// </summary>
            /// <param name="args">
            ///     Arguments to check the length of.
            /// </param>
            public static void IfNoArgs(string[] args)
            {
                if (args.Length == 0) WithError("No arguments provided.", 1);
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
            public static void WithError(string error, int code)
            {
                Message.Show(error, Message.Type.Error);
                Environment.Exit(code);
            }
        }
    }
}