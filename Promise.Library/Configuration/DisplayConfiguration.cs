namespace Promise.Library.Configuration
{
    public class DisplayConfiguration : IConfiguration
    {
        private const string ParameterResolution = "-vidmode";
        private const string ParameterAdapter = "-adapter";
        private const string ParameterWindowMode = "-window";
        private const string ParameterFixedMode = "-useff";

        private readonly int _resolutionX;
        private readonly int _resolutionY;
        private readonly int _refreshRate;
        private readonly int _adapter;

        private readonly bool _isLowend;
        private readonly bool _isWindow;

        public DisplayConfiguration(int width = 800, int height = 600, int refreshRate = 60, int adapter = 1,
            bool isWindow = false, bool isLowend = false)
        {
            _resolutionX = width;
            _resolutionY = height;
            _refreshRate = refreshRate;
            _adapter = adapter;
            _isWindow = isWindow;
            _isLowend = isLowend;
        }

        // Video configuration expressions.
        private string Resolution => $"{ParameterResolution} {_resolutionX},{_resolutionY},{_refreshRate}";

        private string Parameters => $"{ParameterAdapter} {_adapter}";

        // Toggles expressions.
        private string WindowMode => _isWindow ? ParameterWindowMode : string.Empty;

        private string LowendMode => _isLowend ? ParameterFixedMode : string.Empty;

        public string GetConfiguration()
        {
            return $"{Resolution} {Parameters} {WindowMode} {LowendMode}";
        }
    }
}