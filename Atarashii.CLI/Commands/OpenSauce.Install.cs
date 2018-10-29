using System;
using Atarashii.CLI.Outputs;
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
            public override void Initialise(string[] args)
            {
                Argument.ExitIfNone(args);

                var installer = new InstallerFactory(args[0]).Get();
                var installerState = installer.Verify();

                if (installerState.IsValid)
                    Message.Show("Installer verification has passed.", Message.Type.Success);
                else
                    Exit.WithError(installerState.Reason, 4);

                try
                {
                    installer.Install();
                    Message.Show("OpenSauce has been successfully installed.", Message.Type.Success);
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