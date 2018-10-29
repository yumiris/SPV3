using System;
using Atarashii.CLI.Common;
using Atarashii.CLI.Outputs;
using Atarashii.OpenSauce;

namespace Atarashii.CLI.Commands
{
    internal class OpenSauce : Command
    {
        public static void Initialise(string[] commands)
        {
            Exit.IfNoArgs(commands);
            var args = RemoveComFromArgs(commands);

            switch (commands[0])
            {
                case nameof(Install):
                    ShowInvokeMessage(nameof(OpenSauce), nameof(Install));
                    Install.Initialise(args);
                    break;
                default:
                    Exit.WithError($"Invalid '{nameof(OpenSauce)}' command given.", 1);
                    break;
            }
        }

        /// <summary>
        ///     OpenSauce installation sub-command.
        /// </summary>
        private static class Install
        {
            public static void Initialise(string[] args)
            {
                Exit.IfNoArgs(args);

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