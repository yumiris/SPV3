namespace Promise.Library.Configuration
{
    public class ParameterConfiguration : IConfiguration
    {
        private const string ParameterConsoleMode = "-console";
        private const string ParameterDeveloperMode = "-devmode";
        private const string ParameterSafeMode = "-safemode";

        private readonly bool _isConsole;
        private readonly bool _isDeveloper;
        private readonly bool _isSafemode;

        public ParameterConfiguration(bool isConsole = false, bool isDeveloper = false, bool isSafemode = false)
        {
            _isConsole = isConsole;
            _isDeveloper = isDeveloper;
            _isSafemode = isSafemode;
        }

        private string ConsoleMode => _isConsole ? ParameterConsoleMode : string.Empty;
        private string DeveloperMode => _isDeveloper ? ParameterDeveloperMode : string.Empty;
        private string SafeMode => _isSafemode ? ParameterSafeMode : string.Empty;

        public string GetConfiguration()
        {
            return $"{ConsoleMode} {DeveloperMode} {SafeMode}";
        }
    }
}