using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Atarashii.CLI
{
    /// <summary>
    ///     Abstract type representing console output data.
    /// </summary>
    public abstract class Output
    {
        /// <summary>
        ///     Writes an indented line to the console.
        /// </summary>
        /// <param name="value">
        ///     Value to write.
        /// </param>
        protected static void Write(string value)
        {
            Console.Write(new string(' ', 2) + value);
        }

        /// <summary>
        ///     Writes an indented line to the console w/ newline at the end.
        /// </summary>
        /// <param name="value">
        ///     Value to write.
        /// </param>
        protected static void WriteLine(string value)
        {
            Console.WriteLine(new string(' ', 2) + value);
        }

        /// <summary>
        ///     Indents and writes an inbound string with multiple lines.
        /// </summary>
        /// <param name="value">
        ///     Value to write.
        /// </param>
        protected static void WriteLineMulti(string value)
        {
            using (var reader = new StringReader(value))
            {
                string line;
                while ((line = reader.ReadLine()) != null) WriteLine(line);
            }

            WriteLine(string.Empty);
        }
    }
}