using System;
using System.IO;

namespace Promise.Library
{
    public class Configuration
    {
        public string ExecutableName { get; set; }

        public bool IsWindow { get; set; } = false;
        public bool IsConsole { get; set; } = false;
        public bool IsDev { get; set; } = false;
        public bool IsSafeMode { get; set; } = false;
        public bool IsLowEnd { get; set; } = false;

        public int ResolutionX { get; set; } = 640;
        public int ResolutionY { get; set; } = 480;
        public int RefreshRate { get; set; } = 60;

        public void WriteConfiguration()
        {
            string videoConfiguration = $"{GetResolution()} {GetWindowMode()}";
            string consoleConfiguration = $"{GetConsoleMode()} {GetDeveloperMode()}";
            string lowendConfiguration = $"{GetSafeMode()} {GetLowEndMode()}";

            using (StreamWriter configFile = new StreamWriter("config.ini"))
            {
                configFile.Write($"{Halo.ExeName} {videoConfiguration} {consoleConfiguration} {lowendConfiguration}");
            }
        }

        private string GetResolution()
        {
            return $"-vidmode {ResolutionX},{ResolutionY},{RefreshRate}";
        }

        private string GetWindowMode()
        {
            return IsWindow ? "-window" : string.Empty;
        }

        private string GetConsoleMode()
        {
            return IsConsole ? "-console" : string.Empty;
        }

        private string GetDeveloperMode()
        {
            return IsDev ? "-devmode" : string.Empty;
        }

        private string GetSafeMode()
        {
            return IsSafeMode ? "-safemode" : string.Empty;
        }

        private string GetLowEndMode()
        {
            return IsLowEnd ? "-useff" : string.Empty;
        }
    }
}
