using System;
using System.IO;
using Atarashii.CLI;

namespace Atarashii.Profile.CLI
{
    /// <summary>
    ///     CLI front-end for loading a lastprof.txt file.
    /// </summary>
    internal class Program : BaseProgram
    {
        public static void Main(string[] args)
        {
            ExitIfNilArgs(args);

            if (!File.Exists(args[0]))
                ExitWithError("File does not exist.", 2);

            try
            {
                var result = new Lastprof(File.ReadAllText(args[0])).Parse();
                Console.WriteLine(result);
            }
            catch (ProfileException e)
            {
                ExitWithError(e.Message, 3);
            }
        }
    }
}