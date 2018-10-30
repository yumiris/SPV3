using System;
using Atarashii.CLI.Outputs;
using Atarashii.Loader;

namespace Atarashii.CLI.Commands
{
    internal partial class Loader
    {
        /// <inheritdoc />
        /// <summary>
        ///     HCE executable loading sub-command.
        /// </summary>
        private class Load : Command
        {
            private readonly Atarashii.Output _output;

            public Load(Atarashii.Output output)
            {
                _output = output;
            }

            public override void Initialise(string[] args)
            {
                Argument.ExitIfNone(args);

                var executable = new Executable(args[0]);
                var executableState = executable.Verify();

                if (executableState.IsValid)
                    _output?.Write(Atarashii.Output.Type.Success, $"{nameof(Loader)}::{nameof(Load)}",
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
        }
    }
}