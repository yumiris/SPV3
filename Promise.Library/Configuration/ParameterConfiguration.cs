namespace Promise.Library.Configuration
{
    public class ParameterConfiguration : Configuration
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

        private string ConsoleMode => GetParameterString(ParameterConsoleMode, _isConsole);
        private string DeveloperMode => GetParameterString(ParameterDeveloperMode, _isDeveloper);
        private string SafeMode => GetParameterString(ParameterSafeMode, _isSafemode);

        public override string GetConfiguration()
        {
            return $"{ConsoleMode} {DeveloperMode} {SafeMode}";
        }
    }
}