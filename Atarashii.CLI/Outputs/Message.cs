using System;
using System.Reflection;
using Atarashii.CLI.Common;

namespace Atarashii.CLI.Outputs
{
    public class Message : Output
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

            ShowDecoration(codeColour, codeNaming);
            ShowMessage(message);
        }

        /// <summary>
        ///     Outputs the decorative message code & assembly name.
        /// </summary>
        private static void ShowDecoration(ConsoleColor codeColour, string codeNaming)
        {
            // message code
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("  [");
            Console.ForegroundColor = codeColour;
            Console.Write($" {codeNaming} ");

            // assembly name
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("] |");
            Console.Write($" {Assembly.GetEntryAssembly().GetName().Name} ");
            Console.Write("|");
        }

        /// <summary>
        ///     Outputs the actual message.
        /// </summary>
        private static void ShowMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" " + message);
        }
    }
}