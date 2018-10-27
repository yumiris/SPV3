using System;
using Atarashii.CLI;

namespace Atarashii.OpenSauce.CLI
{
    internal class Program : BaseProgram
    {
        public static void Main(string[] args)
        {
            ShowBanner();
            ExitIfNilArgs(args);
            ShowMessage("Invoked installation to " + args[0] , MessageType.Info);

            try
            {
                new InstallerFactory(args[0]).Get().Install();
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