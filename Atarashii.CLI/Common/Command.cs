using System;
using System.Collections.Generic;
using System.Linq;
using Atarashii.CLI.Outputs;

namespace Atarashii.CLI.Common
{
    /// <summary>
    ///     Abstract type representing an invokable Atarashii CLI command.
    /// </summary>
    public abstract class Command
    {
        /// <summary>
        ///     Removes the command (first argument) from an arguments array.
        /// </summary>
        /// <param name="args">
        ///     Arguments array to remove the command from.
        /// </param>
        /// <returns>
        ///     Arguments array without the command.
        /// </returns>
        protected static string[] RemoveComFromArgs(string[] args)
        {
            return args.Skip(1).ToArray();
        }

        protected static class Exit
        {
            /// <summary>
            ///     Exits the program if the inbound commands are either:
            ///     - don't match any of the arguments in the inbound correct commands
            ///     - lack the proper length defined in the correct commands
            /// </summary>
            /// <param name="commands">
            ///     Commands to validate.
            /// </param>
            /// <param name="correctCommands">
            ///     Dictionary of correct commands where key = command name and value = number of required arguments.
            /// </param>
            public static void IfIncorrectCommands(string[] commands, Dictionary<string, int> correctCommands)
            {
                if (commands.Length == 0) WithError("No command provided.", 1);

                var validCommand = false;
                var enoughLength = false;

                foreach (var correctCommand in correctCommands)
                {
                    if (commands[0] != correctCommand.Key) continue;

                    validCommand = true;
                    if (commands.Length >= correctCommand.Value)
                        enoughLength = true;
                }

                if (!validCommand)
                    WithError("No valid command provided.", 2);

                if (!enoughLength)
                    WithError("Not enough arguments provided.", 3);
            }

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