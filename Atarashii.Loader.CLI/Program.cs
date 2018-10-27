using System;
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
            ShowMessage("Loader has been successfully launched with arguments.", MessageType.Success);

            switch (args[0])
            {
                case "load":
                    ShowMessage("Invoked the load command.", MessageType.Info);
                    HandleLoadCommand(args);
                    break;
                case "detect":
                    ShowMessage("Invoked the detect command.", MessageType.Info);
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
                ExitWithError("No arguments provided for the load command.", 1);

            var executable = new Executable(args[1]);

            ShowMessage("Invoking executable verification.", MessageType.Info);
            var executableState = executable.Verify();

            if (!executableState.IsValid)
                ExitWithError(executableState.Reason, 5);

            ShowMessage("Executable verification has passed.", MessageType.Success);

            try
            {
                ShowMessage("Invoking executable loading.", MessageType.Info);
                new Executable(args[1]).Load();
                ShowMessage("The specified executable has been loaded.", MessageType.Success);
            }
            catch (LoaderException e)
            {
                ExitWithError(e.Message, 3);
            }
            catch (Exception e)
            {
                ExitWithError(e.Message, 4);
            }

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