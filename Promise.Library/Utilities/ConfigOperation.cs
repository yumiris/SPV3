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
            try
            {
                using (var configFile = new StreamReader(FileName))
                {
                    return configFile.ReadToEnd();
                }
            }
            catch
            {
                return "-vidmode 640,480,30";
            }
        }
    }
}