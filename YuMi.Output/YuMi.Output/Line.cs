using System;

namespace YuMi.Output
{
    /// <summary>
    /// Wrapper class for System.Console for pretty output.
    /// </summary>
    public static class Line
    {
        /// <summary>
        /// Writes to the Console.
        /// </summary>
        /// <param name="message">Message to write.</param>
        public static void Write(string message)
        {
            Console.WriteLine(message);
        }

        /// <summary>
        /// Writes to the Console with a given foreground colour.
        /// </summary>
        /// <param name="message">Message to write.</param>
        /// <param name="colour">Message foreground colour.</param>
        public static void Write(string message, ConsoleColor colour)
        {
            Console.ForegroundColor = colour;
            Write(message);
        }
        
        /// <summary>
        /// Writes to the Console with a given foreground colour and prefix at the start.
        /// </summary>
        /// <param name="message">Message to write.</param>
        /// <param name="colour">Message foreground colour.</param>
        /// <param name="prefix">Prefixes such as WARN, INFO, HALT, STEP, DONE.</param>
        public static void Write(string message, ConsoleColor colour, string prefix)
        {
            Console.ForegroundColor = colour;
            Write($"[ {prefix} ] | {message}");
        }
        
        /// <summary>
        /// Writes to the Console with a given foreground colour, prefix at the start, and descriptive title.
        /// </summary>
        /// <param name="message">Message to write.</param>
        /// <param name="colour">Message foreground colour.</param>
        /// <param name="prefix">Prefixes such as WARN, INFO, HALT, STEP, DONE.</param>
        /// <param name="title">The process' or the step's name.</param>
        public static void Write(string message, ConsoleColor colour, string prefix, string title)
        {
            Console.ForegroundColor = colour;
            Write($"[ {prefix} ] | {title} | {message}");
        }
    }
}