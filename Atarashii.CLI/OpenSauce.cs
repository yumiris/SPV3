using System;
using Atarashii.OpenSauce;

namespace Atarashii.CLI
{
    internal class OpenSauce : Program
    {
        public static void Initiate(string[] args)
        {
            ExitIfNoArgs(args);
            ShowMessage("Invoked installation to " + args[0], MessageType.Info);

            var installer = new InstallerFactory(args[0]).Get();
            var installerState = installer.Verify();

            if (!installerState.IsValid)
                ExitWithError(installerState.Reason, 4);

            ShowMessage("Installer verification has passed.", MessageType.Success);

            try
            {
                installer.Install();
                ShowMessage("OpenSauce has been successfully installed.", MessageType.Success);
            }
            catch (OpenSauceException e)
            {
                ExitWithError(e.Message, 2);
            }
            catch (Exception e)
            {
                ExitWithError(e.Message, 3);
            }
        }
    }
}