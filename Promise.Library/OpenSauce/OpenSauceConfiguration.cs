using System;
using System.IO;
using System.Xml.Serialization;

namespace Promise.Library.OpenSauce
{
    public class OpenSauceConfiguration
    {
        public OpenSauce OpenSauce { private get; set; }
        private const string OpenSauceConfigFileName = "OS_Settings.User.xml";

        public void SaveData()
        {
            string openSauceDirectoryPath = GetOsDataDirectoryPath();
            string openSauceFilePath = $"{openSauceDirectoryPath}\\{OpenSauceConfigFileName}";

            Directory.CreateDirectory(openSauceDirectoryPath);

            using (FileStream file = File.Create(openSauceFilePath))
            {
                XmlSerializer writer = new XmlSerializer(typeof(OpenSauce));
                writer.Serialize(file, OpenSauce);
            }
        }

        private static string GetOsDataDirectoryPath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "My Games", "Halo CE", "OpenSauce");
        }
    }
}
