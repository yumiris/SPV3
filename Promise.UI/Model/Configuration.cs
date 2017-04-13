using Promise.Library.Video;

namespace Promise.UI.Model
{
    internal class Configuration
    {
        public VideoAdapter VideoAdapter { get; set; }
        public VideoResolution VideoResolution { get; set; }
        public VideoRefreshRate VideoRefreshRate { get; set; }
        public bool IsWindow { get; set; }
        public bool IsSafeMode { get; set; }
        public bool IsFixedMode { get; set; }
    }
}