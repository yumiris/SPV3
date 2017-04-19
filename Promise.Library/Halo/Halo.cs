using Promise.Library.Halo.Video;

namespace Promise.Library.Halo
{
    public class Halo
    {
        public VideoResolution VideoResolution { get; set; } = new VideoResolution();
        public VideoRefreshRate VideoRefreshRate { get; set; } = new VideoRefreshRate();
        public VideoAdapter VideoAdapter { get; set; } = new VideoAdapter();

        public bool IsWindow { get; set; } = false;
        public bool IsSafeMode { get; set; } = false;
        public bool IsFixedMode { get; set; } = false;
    }
}