using System;
using Atarashii.Executable;

namespace Atarashii.CLI.Loader
{
    /// <summary>
    ///     CLI front-end for loading a HCE executable.
    /// </summary>
    internal class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length == 0) ErrorExit($"The specified executable '{args[0]}' was not found.", 1);

            var hceExe = args[0];
            var loader = new Executable.Loader();
            var verifier = new Verifier();

            try
            {
                loader.Execute(hceExe, verifier);
            }
            catch (LoaderException e)
            {
                ErrorExit(e.Message, 2);
            }
            catch (Exception e)
            {
                ErrorExit(e.Message, 3);
            }

            Console.WriteLine($"The specified executable '{hceExe}' has been loaded.");
            Environment.Exit(0);
        }

        private static void ErrorExit(string error, int code)
        {
            Console.Error.WriteLine(error);
            Environment.Exit(code);
        }
    }
}