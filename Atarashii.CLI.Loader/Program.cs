using System;
using System.IO;
using Atarashii.Exceptions;

namespace Atarashii.CLI.Loader
{
    /// <summary>
    ///     CLI front-end for loading a HCE executable.
    /// </summary>
    internal class Program
    {
        public static void Main(string[] args)
        {
            var executable = new Executable(Executable.Name);

            if (args.Length > 0)
                executable = new Executable(args[0]);
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

        private static void ErrorExit(string error, int code)
        {
            Console.Error.WriteLine(error);
            Environment.Exit(code);
        }
    }
}