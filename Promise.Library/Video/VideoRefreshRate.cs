using System.Collections.Generic;

namespace Promise.Library.Video
{
    public class VideoRefreshRate
    {
        public int Rate { get; set; } = 60;

        public string Description => $"{Rate}Hz";

        public List<VideoRefreshRate> GameRefreshRates => new List<VideoRefreshRate>
        {
            new VideoRefreshRate {Rate = 120},
            new VideoRefreshRate {Rate = 60},
            new VideoRefreshRate {Rate = 30}
        };
    }
}