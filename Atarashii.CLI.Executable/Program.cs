using System;
using System.IO;
using Atarashii.Exceptions;

namespace Atarashii.CLI.Executable
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                ErrorExit("No commands provided.", 1);
            }

            switch (args[0])
            {
                case "load":
                    HandleLoadCommand(args);
                    break;
                case "detect":
                    HandleDetectCommand();
                    break;
                default:
                    ErrorExit("Invalid command provided.", 1);
                    break;
            }
        }

        private static void HandleLoadCommand(string[] args)
        {
            var executable = new Atarashii.Executable(Atarashii.Executable.Name);

            if (args.Length > 1)
                executable = new Atarashii.Executable(args[1]);
            else
                try
                {
                    executable = ExecutableFactory.Get(ExecutableFactory.Type.Detect);
                }
                catch (FileNotFoundException e)
                {
                    ErrorExit(e.Message, 1);
                }

            try
            {
                executable.Load();
            }
            catch (LoaderException e)
            {
                ErrorExit(e.Message, 2);
            }
            catch (Exception e)
            {
                ErrorExit(e.Message, 3);
            }

            Console.WriteLine($"The specified executable '{executable.Path}' has been loaded.");
            Environment.Exit(0);
        }

        private static void HandleDetectCommand()
        {
            try
            {
                Console.WriteLine(ExecutableFactory.Get(ExecutableFactory.Type.Detect).Path);
                Environment.Exit(0);
            }
            catch (FileNotFoundException e)
            {
                ErrorExit(e.Message, 4);
                Console.Error.WriteLine(e.Message);
            }
        }

        private static void ErrorExit(string error, int code)
        {
            Console.Error.WriteLine(error);
            Environment.Exit(code);
        }
    }
}