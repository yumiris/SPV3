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
            ExitIfNone(commands);
            var args = FromCommand(commands);

            switch (commands[0])
            {
                case nameof(Install):
                    Install(args);
                    break;
                default:
                    Fail("Invoked an invalid OpenSauce command.", 2);
                    break;
            }
        }

        private void Install(string[] args)
        {
            Info("Invoked the OpenSauce.Install command.");
            ExitIfNone(args);

            var installer = new InstallerFactory(args[0]).Get();
            var installerState = installer.Verify();

            if (installerState.IsValid)
                Pass("Installer verification has passed.");
            else
                Fail(installerState.Reason, 4);

            try
            {
                installer.Install();
                Pass("OpenSauce has been successfully installed.");
            }
            catch (OpenSauceException e)
            {
                Fail(e.Message, 2);
            }
            catch (Exception e)
            {
                Fail(e.Message, 3);
            }
        }
    }
}