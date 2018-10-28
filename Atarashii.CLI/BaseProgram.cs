using System;
using System.Diagnostics;
using System.Reflection;

namespace Atarashii.CLI
{
    /// <summary>
    ///     Abstract with shared methods for CLI Programs.
    /// </summary>
    public abstract class BaseProgram
    {
        protected enum MessageType
        {
            /// <summary>
            ///     Represents a successful step.
            /// </summary>
            Success,

            /// <summary>
            ///     Represents an informative message.
            /// </summary>
            Info,

            /// <summary>
            ///     Represents an error message which should outputted to STDERR.
            /// </summary>
            Error
        }

        /// <summary>
        ///     File information of the calling assembly.
        /// </summary>
        private static readonly FileVersionInfo Info =
            FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);

        /// <summary>
        ///     CLI-friendly ASCII art banner.
        /// </summary>
        private static string Banner => $@"
         _                      _     _ _ 
    __ _| |_ __ _ _ __ __ _ ___| |__ (_|_)
   / _` | __/ _` | '__/ _` / __| '_ \| | |
  | (_| | || (_| | | | (_| \__ \ | | | | |
   \__,_|\__\__,_|_|  \__,_|___/_| |_|_|_|

  Program    : {Info.ProductName}
  Developers : {Info.CompanyName}
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
        ///     Outputs decorative messages to the CLI.
        /// </summary>
        /// <param name="message">
        ///     Message to output.
        /// </param>
        /// <param name="messageType">
        ///     Type of message. Affects colour and decorators.
        ///     For available message types: <see cref="MessageType" />
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Invalid message type value.
        /// </exception>
        protected static void ShowMessage(string message, MessageType messageType)
        {
            ConsoleColor codeColour;
            string codeNaming;
            var isErrorMsg = false;

            switch (messageType)
            {
                case MessageType.Success:
                    codeColour = ConsoleColor.Green;
                    codeNaming = " OK ";
                    break;
                case MessageType.Info:
                    codeColour = ConsoleColor.Cyan;
                    codeNaming = "INFO";
                    break;
                case MessageType.Error:
                    codeColour = ConsoleColor.Red;
                    codeNaming = "HALT";
                    isErrorMsg = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(messageType), messageType, null);
            }

            if (isErrorMsg)
            {
                // indent without altering error message
                Console.Write("  ");
                Console.Error.WriteLine(message);
            }

            Console.WriteLine(string.Empty);

            // left decoration
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("  [");
            
            // output code (HALT, INFO, etc.)
            Console.ForegroundColor = codeColour;
            Console.Write($" {codeNaming} ");

            // right decoration
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("] |");
            Console.Write($" {Assembly.GetEntryAssembly().GetName().Name} ");
            Console.Write("|");

            // output actual message 
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" " + message);
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
            ShowMessage(error, MessageType.Error);
            Environment.Exit(code);
        }
    }
}