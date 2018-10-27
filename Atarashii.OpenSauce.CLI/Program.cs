using Atarashii.CLI;

namespace Atarashii.OpenSauce.CLI
{
    internal class Program : BaseProgram
    {
        public static void Main(string[] args)
        {
            ExitIfNilArgs(args);

            var installer = new InstallerFactory(args[0]).Get();
            var installerState = installer.Verify();

            if (!installerState.IsValid) ExitWithError(installerState.Reason, 2);
        }
    }
}