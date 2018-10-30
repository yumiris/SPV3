using System;
using Atarashii.OpenSauce;

namespace Atarashii.CLI.Commands
{
    /// <inheritdoc />
    internal class OpenSauce : Command
    {
        public OpenSauce(Output output) : base(output)
        {
        }

        public override void Initialise(string[] commands)
        {
            Argument.ExitIfNone(commands);
            var args = Argument.FromCommand(commands);

            switch (commands[0])
            {
                case nameof(Install):
                    OutputMessage(Output.Type.Success, "Invoked the OpenSauce.Install command.");
                    Install(args);
                    break;
                default:
                    OutputMessage(Output.Type.Error, "Invoked an invalid OpenSauce command.");
                    break;
            }
        }

        private void Install(string[] args)
        {
            Argument.ExitIfNone(args);

            var installer = new InstallerFactory(args[0]).Get();
            var installerState = installer.Verify();

            if (installerState.IsValid)
                OutputMessage(Output.Type.Success, "Installer verification has passed.");
            else
                Exit.WithError(installerState.Reason, 4);

            try
            {
                installer.Install();
                OutputMessage(Output.Type.Success, "OpenSauce has been successfully installed.");
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