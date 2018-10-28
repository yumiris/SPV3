using System;
using System.Reflection;

namespace Atarashii.CLI
{
    public static class Message
    {
        /// <summary>
        ///     Various message types that are outputted in the console.
        /// </summary>
        public enum Type
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
        ///     Outputs decorative messages to the CLI.
        /// </summary>
        /// <param name="message">
        ///     Message to output.
        /// </param>
        /// <param name="type">
        ///     Type of message. Affects colour and decorators.
        ///     For available message types: <see cref="Type" />
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Invalid message type value.
        /// </exception>
        public static void Show(string message, Type type)
        {
            ConsoleColor codeColour;
            string codeNaming;
            var isErrorMsg = false;

            switch (type)
            {
                case Type.Success:
                    codeColour = ConsoleColor.Green;
                    codeNaming = " OK ";
                    break;
                case Type.Info:
                    codeColour = ConsoleColor.Cyan;
                    codeNaming = "INFO";
                    break;
                case Type.Error:
                    codeColour = ConsoleColor.Red;
                    codeNaming = "HALT";
                    isErrorMsg = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
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
    }
}