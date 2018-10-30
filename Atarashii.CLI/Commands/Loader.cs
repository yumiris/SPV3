using System;
using System.IO;
using Atarashii.Loader;

namespace Atarashii.CLI.Commands
{
    /// <inheritdoc />
    /// <summary>
    ///     CLI front-end for loading a HCE executable.
    /// </summary>
    internal class Loader : Command
    {
        public Loader(Output output) : base(output)
        {
        }

        public override void Initialise(string[] commands)
        {
            Argument.ExitIfNone(commands);
            var args = Argument.FromCommand(commands);

            switch (commands[0])
            {
                case nameof(Load):
                    OutputMessage(Output.Type.Success, "Invoked the Loader.Load command.");
                    Load(args);
                    break;
                case nameof(Detect):
                    OutputMessage(Output.Type.Success, "Invoked the Loader.Detect command.");
                    Detect();
                    break;
                default:
                    OutputMessage(Output.Type.Error, "Invoked an invalid Load command.");
                    break;
            }
        }

        private void Load(string[] args)
        {
            Argument.ExitIfNone(args);

            var executable = new Executable(args[0]);
            var executableState = executable.Verify();

            if (executableState.IsValid)
                OutputMessage(Output.Type.Success, "Executable verification has passed.");
            else
                Exit.WithError(executableState.Reason, 5);

            try
            {
                executable.Load();
                OutputMessage(Output.Type.Success, "The specified executable has been loaded.");
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

        private void Detect()
        {
            OutputMessage(Output.Type.Info, "Attempting to detect executable path.");

            try
            {
                var result = ExecutableFactory.Get(ExecutableFactory.Type.Detect);
                OutputMessage(Output.Type.Success, "Profile name successfully parsed:");
                Console.WriteLine(result);
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