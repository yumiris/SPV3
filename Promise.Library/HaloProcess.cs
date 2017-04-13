using System.Diagnostics;
using Promise.Library.Configuration;

namespace Promise.Library
{
    public class HaloProcess
    {
        public const string ExeName = "haloce.exe";
        public HaloConfiguration Configuration { private get; set; }

        public void Launch()
        {
            Process.Start(ExeName, Configuration.ReadConfiguration());
        }
    }
}