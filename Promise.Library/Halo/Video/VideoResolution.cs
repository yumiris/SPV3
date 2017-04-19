using System.Collections.Generic;

namespace Promise.Library.Halo.Video
{
    public class VideoResolution
    {
        public int Width { get; set; } = 800;
        public int Height { get; set; } = 600;

        public string Description => $"{Width} x {Height}";

        public List<VideoResolution> GetVideoResolutions()
        {
            return new List<VideoResolution>
            {
                new VideoResolution {Width = 1920, Height = 1080},
                new VideoResolution {Width = 1680, Height = 1050},
                new VideoResolution {Width = 1600, Height = 1200},
                new VideoResolution {Width = 1600, Height = 900},
                new VideoResolution {Width = 1440, Height = 900},
                new VideoResolution {Width = 1366, Height = 768},
                new VideoResolution {Width = 1360, Height = 768},
                new VideoResolution {Width = 1280, Height = 1024},
                new VideoResolution {Width = 1280, Height = 800},
                new VideoResolution {Width = 1280, Height = 768},
                new VideoResolution {Width = 1280, Height = 720},
                new VideoResolution {Width = 1152, Height = 864},
                new VideoResolution {Width = 1024, Height = 768},
                new VideoResolution {Width = 1024, Height = 600},
                new VideoResolution {Width = 800, Height = 600},
                new VideoResolution {Width = 640, Height = 480}
            };
        }
    }
}