using System;
using System.IO;

namespace Promise.Library.Serialisation
{
    public class OpenSauceXml
    {
        public void CreateConfigurationDirectory()
        {
            Directory.CreateDirectory(GetConfigurationDirectory());
        }

        private string GetConfigurationDirectory()
        {
            string myDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string filePath = Path.Combine(myDocuments, "My Games", "Halo CE", "OpenSauce");

            return filePath;
        }

        public string GetConfigurationFilename()
        {
            return $"{GetConfigurationDirectory()}\\OS_Settings.User.xml";
        }
    }
}