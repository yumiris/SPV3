using Promise.Library.Properties;

namespace Promise.Library.Configuration
{
    public class VideoConfiguration : IConfiguration
    {
        private readonly int _resolutionX;
        private readonly int _resolutionY;
        private readonly int _refreshRate;

        private readonly int _adapter;

        private readonly bool _isWindow;
        private readonly bool _isLowend;

        // Video configuration expressions.
        private string Resolution => $"{Resources.Resolution} {_resolutionX},{_resolutionY},{_refreshRate}";
        private string Parameters => $"{Resources.Adapter} {_adapter}";

        // Toggles expressions.
        private string WindowMode => (_isWindow) ? Resources.WindowMode : string.Empty;
        private string LowendMode => (_isLowend) ? Resources.LowendMode : string.Empty;

        public VideoConfiguration(int width = 800, int height = 600, int refreshRate = 60, int adapter = 1, bool isWindow = false, bool isLowend = false)
        {
            _resolutionX = width;
            _resolutionY = height;
            _refreshRate = refreshRate;
            _adapter = adapter;
            _isWindow = isWindow;
            _isLowend = isLowend;
        }

        public virtual string GetConfiguration()
        {
            return $"{Resolution} {Parameters} {WindowMode} {LowendMode}";
        }
    }
}
