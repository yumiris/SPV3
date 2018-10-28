using System;
using System.IO;
using Atarashii.CLI.Common;
using Atarashii.CLI.Outputs;
using Atarashii.Loader;

namespace Atarashii.CLI.Commands
{
    /// <summary>
    ///     CLI front-end for loading a HCE executable.
    /// </summary>
    internal class Loader : Command
    {
        public static void Initiate(string[] args)
        {
            Exit.IfNoArgs(args);

            var command = args[0].ToLower();

            switch (command)
            {
                case "load":
                    Message.Show($"Invoked the load command on '{args[1]}'.", Message.Type.Info);
                    HandleLoadCommand(RemoveComFromArgs(args));
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

        private static void HandleLoadCommand(string[] args)
        {
            Exit.IfNoArgs(args);

            var executable = new Executable(args[0]);
            var executableState = executable.Verify();

            if (!executableState.IsValid) Exit.WithError(executableState.Reason, 5);

            Message.Show("Executable verification has passed.", Message.Type.Success);

            try
            {
                executable.Load();
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