using System;
using Atarashii.OpenSauce;

namespace Atarashii.CLI.Commands
{
    /// <inheritdoc />
    internal class OpenSauce : Command
    {
        private readonly Atarashii.Output _output;

        public OpenSauce(Atarashii.Output output)
        {
            _output = output;
        }

        public override void Initialise(string[] commands)
        {
            Argument.ExitIfNone(commands);
            var args = Argument.FromCommand(commands);

            switch (commands[0])
            {
                case nameof(Install):
                    HandleInvokeType(InvokeType.Success, this, nameof(Install));
                    Install(args);
                    break;
                default:
                    HandleInvokeType(InvokeType.Invalid, this, commands[0]);
                    break;
            }
        }

        private void Install(string[] args)
        {
            Argument.ExitIfNone(args);

            var installer = new InstallerFactory(args[0]).Get();
            var installerState = installer.Verify();

            if (installerState.IsValid)
                _output?.Write(Atarashii.Output.Type.Success, $"{nameof(OpenSauce)}.Install",
                    "Installer verification has passed.");
            else
                Exit.WithError(installerState.Reason, 4);

            try
            {
                installer.Install();
                _output?.Write(Atarashii.Output.Type.Success, $"{nameof(OpenSauce)}.Install",
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