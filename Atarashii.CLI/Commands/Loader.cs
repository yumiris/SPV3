using System;
using System.Collections.Generic;
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
        private const string LoadCommand = "load";
        private const string DetectCommand = "detect";

        private static Dictionary<string, int> Available { get; } = new Dictionary<string, int>
        {
            {LoadCommand, 1},
            {DetectCommand, 0}
        };

        public static void Initiate(string[] commands)
        {
            Exit.IfIncorrectCommands(commands, Available);

            var command = commands[0].ToLower();
            var args = RemoveComFromArgs(commands);

            switch (command)
            {
                case LoadCommand:
                    HandleLoadCommand(args);
                    break;
                case DetectCommand:
                    HandleDetectCommand();
                    break;
            }
        }

        private static void HandleLoadCommand(string[] args)
        {
            Exit.IfNoArgs(args);
            Message.Show($"Invoked the {LoadCommand} command on '{args[0]}'.", Message.Type.Info);

            var executable = new Executable(args[0]);
            var executableState = executable.Verify();

            if (executableState.IsValid)
                Message.Show("Executable verification has passed.", Message.Type.Success);
            else
                Exit.WithError(executableState.Reason, 5);

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
            Message.Show($"Invoked the {DetectCommand} command.", Message.Type.Info);

            try
            {
                Console.WriteLine(ExecutableFactory.Get(ExecutableFactory.Type.Detect));
                Environment.Exit(0);
            }
            catch (FileNotFoundException e)
            {
                Exit.WithError(e.Message, 3);
                Environment.Exit(5);
            }
        }
    }
}