using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Promise.Library.OpenSauce
{
    public class OpenSauceConfiguration
    {
        private const string OpenSauceConfigFileName = "OS_Settings.User.xml";

        public void Serialise(OpenSauce openSauce)
        {
            CreateOpenSauceDirectory();

            using (var file = File.Create(GetOpenSauceFilePath()))
            {
                var writer = new XmlSerializer(typeof(OpenSauce));
                writer.Serialize(file, openSauce);
            }
        }

        public OpenSauce GetDeserialisedOpenSauce()
        {
            var xmlSerializer = new XmlSerializer(typeof(OpenSauce));
            OpenSauce deserialisedOpenSauce;

            if (!File.Exists(GetOpenSauceFilePath()))
            {
                Serialise(new OpenSauce());
            }

            using (var reader = XmlReader.Create(GetOpenSauceFilePath()))
            {
                deserialisedOpenSauce = (OpenSauce) xmlSerializer.Deserialize(reader);
            }

            return deserialisedOpenSauce;
        }

        private static void CreateOpenSauceDirectory()
        {
            Directory.CreateDirectory(GetOpenSauceDirectoryPath());
        }

        private string GetOpenSauceFilePath()
        {
            return $"{GetOpenSauceDirectoryPath()}\\{OpenSauceConfigFileName}";
        }

        private static string GetOpenSauceDirectoryPath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "My Games", "Halo CE",
                "OpenSauce");
        }
    }
}