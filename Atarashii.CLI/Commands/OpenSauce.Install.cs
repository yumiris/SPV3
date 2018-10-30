using System;
using Atarashii.CLI.Outputs;
using Atarashii.Loader;
using Atarashii.OpenSauce;

namespace Atarashii.CLI.Commands
{
    internal partial class OpenSauce
    {
        /// <inheritdoc />
        /// <summary>
        ///     OpenSauce installation sub-command.
        /// </summary>
        private class Install : Command
        {
            private readonly Atarashii.Output _output;

            public Install(Atarashii.Output output)
            {
                _output = output;
            }

            public override void Initialise(string[] args)
            {
                Argument.ExitIfNone(args);

                var installer = new InstallerFactory(args[0]).Get();
                var installerState = installer.Verify();

                if (installerState.IsValid)
                    _output?.Write(Atarashii.Output.Type.Success, $"{nameof(OpenSauce)}::{nameof(Install)}",
                        "Installer verification has passed.");
                else
                    Exit.WithError(installerState.Reason, 4);

                try
                {
                    installer.Install();
                    _output?.Write(Atarashii.Output.Type.Success,  $"{nameof(OpenSauce)}::{nameof(Install)}",
                        "OpenSauce has been successfully installed.");
                }
                catch (OpenSauceException e)
                {
                    Exit.WithError(e.Message, 2);
                }
                catch (Exception e)
                {
                    Exit.WithError(e.Message, 3);
                }
            }
        }
    }
}