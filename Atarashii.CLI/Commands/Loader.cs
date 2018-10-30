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
                    Info("Invoked the Loader.Load command.");
                    Load(args);
                    break;
                case nameof(Detect):
                    Info("Invoked the Loader.Detect command.");
                    Detect();
                    break;
                default:
                    Fail("Invoked an invalid Load command.", 1);
                    break;
            }
        }

        private void Load(string[] args)
        {
            Argument.ExitIfNone(args);

            var executable = new Executable(args[0]);
            var executableState = executable.Verify();

            if (executableState.IsValid)
                Pass("Executable verification has passed.");
            else
                Fail(executableState.Reason, 5);

            try
            {
                executable.Load();
                Pass("The specified executable has been loaded.");
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
            Info("Attempting to detect executable path.");

            try
            {
                var result = ExecutableFactory.Get(ExecutableFactory.Type.Detect);
                Pass("Profile name successfully parsed:");
                Console.WriteLine(result);
                Environment.Exit(0);
            }
            catch (FileNotFoundException e)
            {
                Fail(e.Message, 3);
            }
        }
    }
}