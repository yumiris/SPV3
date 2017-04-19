using Promise.Library.Halo;
using Promise.Library.Halo.Video;

namespace Promise.UI.Model
{
    internal class HaloConfiguration
    {
        public VideoAdapter VideoAdapter { get; set; }
        public VideoResolution VideoResolution { get; set; }
        public VideoRefreshRate VideoRefreshRate { get; set; }
        public bool IsWindow { get; set; }
        public bool IsSafeMode { get; set; }
        public bool IsFixedMode { get; set; }


        public void GetValuesFromInstance(Halo halo)
        {
            VideoResolution = halo.VideoResolution;
            VideoRefreshRate = halo.VideoRefreshRate;
            VideoAdapter = halo.VideoAdapter;
            IsWindow = halo.IsWindow;
            IsSafeMode = halo.IsSafeMode;
            IsFixedMode = halo.IsFixedMode;
        }
    }
}