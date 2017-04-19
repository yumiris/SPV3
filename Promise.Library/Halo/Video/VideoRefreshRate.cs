using System.Collections.Generic;

namespace Promise.Library.Halo.Video
{
    public class VideoRefreshRate
    {
        public int Rate { get; set; } = 60;

        public string Description => $"{Rate}Hz";

        public List<VideoRefreshRate> GetVideoRefreshRates()
        {
            return new List<VideoRefreshRate>
            {
                new VideoRefreshRate {Rate = 120},
                new VideoRefreshRate {Rate = 60},
                new VideoRefreshRate {Rate = 30}
            };
        }
    }
}