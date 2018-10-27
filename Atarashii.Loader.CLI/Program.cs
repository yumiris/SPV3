﻿using System;
using System.Collections.Generic;
using System.IO;
using Atarashii.CLI;

namespace Atarashii.Loader.CLI
{
    /// <summary>
    ///     CLI front-end for loading a HCE executable.
    /// </summary>
    internal class Program : BaseProgram
    {
        public static void Main(string[] args)
        {
            ShowBanner();
            ExitIfNilArgs(args);

            switch (args[0])
            {
                case "load":
                    HandleLoadCommand(args);
                    break;
                case "detect":
                    HandleDetectCommand();
                    break;
                default:
                    ExitWithError("Invalid arguments provided.", 2);
                    break;
            }
        }

        private static void HandleLoadCommand(IReadOnlyList<string> args)
        {
            if (args.Count < 2)
                ExitWithError("Not arguments provided for the load command.", 1);

            try
            {
                new Executable(args[1]).Load();
            }
            catch (LoaderException e)
            {
                ExitWithError(e.Message, 3);
            }
            catch (Exception e)
            {
                ExitWithError(e.Message, 4);
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
    }
}