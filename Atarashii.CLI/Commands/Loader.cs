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
        private readonly Atarashii.Output _output;

        public Loader(Atarashii.Output output)
        {
            _output = output;
        }

        public override void Initialise(string[] commands)
        {
            Argument.ExitIfNone(commands);
            var args = Argument.FromCommand(commands);

            switch (commands[0])
            {
                case nameof(Load):
                    HandleInvokeType(InvokeType.Success, this, nameof(Load));
                    Load(args);
                    break;
                case nameof(Detect):
                    HandleInvokeType(InvokeType.Success, this, nameof(Detect));
                    Detect();
                    break;
                default:
                    HandleInvokeType(InvokeType.Invalid, this, commands[0]);
                    break;
            }
        }

        private void Load(string[] args)
        {
            Argument.ExitIfNone(args);

            var executable = new Executable(args[0]);
            var executableState = executable.Verify();

            if (executableState.IsValid)
                _output?.Write(Atarashii.Output.Type.Success, $"{nameof(Loader)}.{nameof(Load)}",
                    "Executable verification has passed.");
            else
                Exit.WithError(executableState.Reason, 5);

            try
            {
                executable.Load();
                _output?.Write(Atarashii.Output.Type.Success, Assembly.ProductName,
                    "The specified executable has been loaded.");
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
            _output?.Write(Atarashii.Output.Type.Info, $"{nameof(Loader)}.{nameof(Detect)}",
                "Attempting to detect executable path.");

            try
            {
                var result = ExecutableFactory.Get(ExecutableFactory.Type.Detect);
                _output?.Write(Atarashii.Output.Type.Success, $"{nameof(Loader)}.{nameof(Detect)}",
                    "Profile name successfully parsed:");
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