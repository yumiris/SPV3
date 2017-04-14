namespace Promise.Library.Configuration
{
    public class DisplayConfiguration : Configuration
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

        private string Resolution => $"{ParameterResolution} {_resolutionX},{_resolutionY},{_refreshRate}";
        private string Parameters => $"{ParameterAdapter} {_adapter}";
        private string WindowMode => GetParameterString(ParameterWindowMode, _isWindow);
        private string LowendMode => GetParameterString(ParameterFixedMode, _isLowend);

        public override string GetConfiguration()
        {
            return $"{Resolution} {Parameters} {WindowMode} {LowendMode}";
        }
    }
}