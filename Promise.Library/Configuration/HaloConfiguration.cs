using System.IO;

namespace Promise.Library.Configuration
{
    public class HaloConfiguration
    {
        private const string ConfigurationFile = "config.ini";

        public void WriteConfiguration(string configurationData)
        {
            using (StreamWriter configFile = new StreamWriter(ConfigurationFile))
            {
                configFile.Write(configurationData);
            }
        }

        public string ReadConfiguration()
        {
            using (StreamReader configFile = new StreamReader(ConfigurationFile))
            {
                return configFile.ReadToEnd();
            }
        }
    }
}