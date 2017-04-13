using System.IO;

namespace Promise.Library.Utilities
{
    public class ConfigOperation
    {
        public const string FileName = "config.ini";

        public void WriteConfiguration(string configurationData)
        {
            using (var configFile = new StreamWriter(FileName))
            {
                configFile.Write(configurationData);
            }
        }

        public string ReadConfiguration()
        {
            using (var configFile = new StreamReader(FileName))
            {
                return configFile.ReadToEnd();
            }
        }
    }
}