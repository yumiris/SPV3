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

            try
            {
                new InstallerFactory(args[0]).Get().Install();
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