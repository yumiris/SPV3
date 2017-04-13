using System.IO;

namespace Promise.Library.Configuration
{
    public class HaloConfiguration
    {
        public const string ConfigurationFile = "config.ini";

        public void WriteConfiguration(string configurationData)
        {
            using (var configFile = new StreamWriter(ConfigurationFile))
            {
                configFile.Write(configurationData);
            }
        }

        public string ReadConfiguration()
        {
            using (var configFile = new StreamReader(ConfigurationFile))
            {
                return configFile.ReadToEnd();
            }
        }
    }
}