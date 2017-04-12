using System.Diagnostics;

namespace Promise.Library
{
    public class Halo
    {
        public const string ExeName = "haloce.exe";

        public void LaunchGame(string launchParameters)
        {
            Process.Start(ExeName, launchParameters);
        }
    }
}