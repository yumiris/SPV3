using System.Diagnostics;
using System.IO;
using Promise.Library.Halo.Video;
using Promise.Library.Utilities;

namespace Promise.Library.Halo
{
    public class Halo
    {
        private const string ExeName = "haloce.exe";

        public VideoResolution VideoResolution { get; set; } = new VideoResolution();
        public VideoRefreshRate VideoRefreshRate { get; set; } = new VideoRefreshRate();
        public VideoAdapter VideoAdapter { get; set; } = new VideoAdapter();

        public bool IsWindow { get; set; } = false;
        public bool IsSafeMode { get; set; } = false;
        public bool IsFixedMode { get; set; } = false;

        public void Launch(ConfigOperation configOperation)
        {
            if (File.Exists(ExeName))
                Process.Start(ExeName, configOperation.ReadConfiguration());
            else
                throw new FileNotFoundException("Halo executable has not been found in this directory.");
        }
    }
}