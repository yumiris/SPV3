using System;
using System.Collections.Generic;
using System.IO;
using Atarashii.Loader;

namespace Atarashii.CLI
{
    /// <summary>
    ///     CLI front-end for loading a HCE executable.
    /// </summary>
    internal class Loader : Program
    {
        public static void Initiate(string[] args)
        {
            Exit.IfNoArgs(args);

            switch (args[0])
            {
                case "load":
                    Message.Show("Invoked the load command.", Message.Type.Info);
                    HandleLoadCommand(args);
                    break;
                case "detect":
                    Message.Show("Invoked the detect command.", Message.Type.Info);
                    HandleDetectCommand();
                    break;
                default:
                    Exit.WithError("Invalid arguments provided.", 2);
                    break;
            }
        }

        private static void HandleLoadCommand(IReadOnlyList<string> args)
        {
            if (args.Count < 2) Exit.WithError("No arguments provided for the load command.", 1);

            var executable = new Executable(args[1]);
            var executableState = executable.Verify();

            if (!executableState.IsValid) Exit.WithError(executableState.Reason, 5);

            Message.Show("Executable verification has passed.", Message.Type.Success);

            try
            {
                new Executable(args[1]).Load();
                Message.Show("The specified executable has been loaded.", Message.Type.Success);
            }
            catch (LoaderException e)
            {
                Exit.WithError(e.Message, 3);
            }
            catch (Exception e)
            {
                Exit.WithError(e.Message, 4);
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