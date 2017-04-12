using System.IO;

namespace Promise.Library
{
    public class Configuration
    {
        private const string ConfigurationFile = "config.ini";

        // Common toggling parameters.
        public bool IsWindow { get; set; } = false;
        public bool IsConsole { get; set; } = false;
        public bool IsDev { get; set; } = false;
        public bool IsSafeMode { get; set; } = false;
        public bool IsLowEnd { get; set; } = false;
        public bool CanScreenshot { get; set; } = false;

        // Display configurations.
        public int ResolutionX { get; set; } = 640;
        public int ResolutionY { get; set; } = 480;
        public int RefreshRate { get; set; } = 60;
        public int Adapter { get; set; } = 1;

        public void WriteConfiguration()
        {
            using (StreamWriter configFile = new StreamWriter(ConfigurationFile))
            {
                configFile.Write($"{GetVideoConfigurations()}{GetLaunchToggles()}");
            }
        }

        public string ReadConfiguration()
        {
            using (StreamReader configFile = new StreamReader(ConfigurationFile))
            {
                return configFile.ReadToEnd();
            }
        }

        private string GetVideoConfigurations()
        {
            string[] videoStrings =
            {
                $"-vidmode {ResolutionX},{ResolutionY},{RefreshRate}",
                $"-adapter {Adapter}",
                IsWindow ? "-window" : string.Empty,
                IsLowEnd ? "-useff" : string.Empty,
            };

            return GetStringsFromArray(videoStrings);
        }

        private string GetLaunchToggles()
        {
            string[] launchStrings =
            {
                IsConsole ? "-console" : string.Empty,
                IsDev ? "-devmode" : string.Empty,
                IsSafeMode ? "-safemode" : string.Empty,
                CanScreenshot ? "-screenshot" : string.Empty
            };

            return GetStringsFromArray(launchStrings);
        }

        private string GetStringsFromArray(string[] stringsArray)
        {
            return string.Join(" ", stringsArray);
        }
    }
}