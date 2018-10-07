using System;
using System.Collections.Generic;
using System.IO;
using Atarashii.Exceptions;

namespace Atarashii.CLI.Executable
{
    /// <summary>
    ///     CLI front-end for loading a HCE executable.
    /// </summary>
    internal class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length == 0)
                ErrorExit("No arguments provided.", 1);

            switch (args[0])
            {
                case "load":
                    HandleLoadCommand(args);
                    break;
                case "detect":
                    HandleDetectCommand();
                    break;
                default:
                    ErrorExit("Invalid arguments provided.", 2);
                    break;
            }
        }

        private static void HandleLoadCommand(IReadOnlyList<string> args)
        {
            if (args.Count < 2)
                ErrorExit("Not arguments provided for the load command.", 1);

            try
            {
                new Atarashii.Executable(args[1]).Load();
            }
            catch (LoaderException e)
            {
                ErrorExit(e.Message, 3);
            }
            catch (Exception e)
            {
                ErrorExit(e.Message, 4);
            }

            Console.WriteLine("The specified executable has been loaded.");
            Environment.Exit(0);
        }

        private static void HandleDetectCommand()
        {
            try
            {
                Console.WriteLine(ExecutableFactory.Get(ExecutableFactory.Type.Detect));
                Environment.Exit(0);
            }
            catch (FileNotFoundException e)
            {
                Console.Error.WriteLine(e.Message);
                Environment.Exit(5);
            }
        }

        private static void ErrorExit(string error, int code)
        {
            Console.Error.WriteLine(error);
            Environment.Exit(code);
        }
    }
}