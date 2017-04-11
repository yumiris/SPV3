using System.IO;

namespace Promise.Library
{
    public class Configuration
    {
        public string ExecutableName { get; set; }

        public void WriteConfiguration()
        {
            using (StreamWriter configFile = new StreamWriter("config.ini"))
            {
                configFile.Write(ExecutableName);
            }
        }
    }
}
