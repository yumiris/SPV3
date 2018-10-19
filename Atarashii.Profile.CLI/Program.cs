using System;
using System.IO;

namespace Atarashii.Profile.CLI
{
    /// <summary>
    ///     CLI front-end for loading a lastprof.txt file.
    /// </summary>
    internal class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length == 0)
                ErrorExit("No arguments provided.", 1);

            if (!File.Exists(args[0]))
                ErrorExit("File does not exist.", 2);

            try
            {
                var result = new Lastprof(File.ReadAllText(args[0])).Parse();
                Console.WriteLine(result);
            }
            catch (ProfileException e)
            {
                ErrorExit(e.Message, 3);
            }
        }

        private static void ErrorExit(string error, int code)
        {
            Console.Error.WriteLine(error);
            Environment.Exit(code);
        }
    }
}