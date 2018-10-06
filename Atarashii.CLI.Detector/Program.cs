using System;

namespace Atarashii.CLI.Detector
{
    /// <summary>
    ///     CLI front-end for detecting the HCE executable.
    /// </summary>
    internal class Program
    {
        public static void Main()
        {
            var path = new Executable().Detect();

            if (string.IsNullOrWhiteSpace(path))
            {
                Console.Error.WriteLine("Legally installed executable not found.");
                Environment.Exit(1);
            }
            else
            {
                Console.WriteLine(path);
                Environment.Exit(0);
            }
        }
    }
}