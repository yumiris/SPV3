using System;
using System.IO;

namespace Atarashii.CLI.Detector
{
    /// <summary>
    ///     CLI front-end for detecting the HCE executable.
    /// </summary>
    internal class Program
    {
        public static void Main()
        {
            try
            {
                Console.WriteLine(ExecutableFactory.Get(ExecutableFactory.Type.Detect));
                Environment.Exit(0);
            }
            catch (FileNotFoundException e)
            {
                Console.Error.WriteLine(e.Message);
                Environment.Exit(1);
            }
        }
    }
}