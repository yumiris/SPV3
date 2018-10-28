using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Atarashii.CLI.Common
{
    /// <summary>
    ///     Abstract type representing console output data.
    /// </summary>
    public abstract class Output
    {   
        /// <summary>
        ///     File information of the calling assembly.
        /// </summary>
        protected static readonly FileVersionInfo Exe =
            FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);

        /// <summary>
        ///     Git repository information.
        /// </summary>
        protected static class Git
        {
            public static string Revision => ReadFromResource("Atarashii.CLI.REVISION");
        }

        /// <summary>
        ///     Reads data from an embedded resource.
        /// </summary>
        /// <param name="resourceName">
        ///    Fully qualified embedded resource name.
        /// </param>
        /// <returns>
        ///    Data read from the embedded resource.
        /// </returns>
        protected static string ReadFromResource(string resourceName)
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        ///     Writes an indented line to the console.
        /// </summary>
        /// <param name="value">
        ///    Value to write.
        /// </param>
        protected static void Write(string value)
        {
            Console.Write(new string(' ', 2) + value);
        }

        /// <summary>
        ///     Writes an indented line to the console w/ newline at the end.
        /// </summary>
        /// <param name="value">
        ///    Value to write.
        /// </param>
        protected static void WriteLine(string value)
        {
            Console.WriteLine(new string(' ', 2) + value);
        }

        /// <summary>
        ///     Indents and writes an inbound string with multiple lines.
        /// </summary>
        /// <param name="value">
        ///    Value to write.
        /// </param>
        protected static void WriteLineMulti(string value)
        {
            using (StringReader reader = new StringReader(value))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    WriteLine(line);
                }
            }
            
            WriteLine(string.Empty);
        }
    }
}