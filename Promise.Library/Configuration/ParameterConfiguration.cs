using Promise.Library.Properties;

namespace Promise.Library.Configuration
{
    public class ParameterConfiguration : IConfiguration
    {
        private readonly bool _isConsole;
        private readonly bool _isDeveloper;
        private readonly bool _isSafemode;

        private string ConsoleMode => (_isConsole) ? Resources.ConsoleMode : string.Empty;
        private string DeveloperMode => (_isDeveloper) ? Resources.DeveloperMode : string.Empty;
        private string SafeMode => (_isSafemode) ? Resources.SafeMode : string.Empty;

        public ParameterConfiguration(bool isConsole = false, bool isDeveloper = false, bool isSafemode = false)
        {
            _isConsole = isConsole;
            _isDeveloper = isDeveloper;
            _isSafemode = isSafemode;
        }

        public virtual string GetConfiguration()
        {
            return $"{ConsoleMode} {DeveloperMode} {SafeMode}";
        }
    }
}
