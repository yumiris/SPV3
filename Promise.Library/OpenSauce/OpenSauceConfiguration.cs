using System.IO;
using System.Xml.Serialization;

namespace Promise.Library.OpenSauce
{
    public class OpenSauceConfiguration
    {
        public OpenSauce OpenSauce { get; set; }

        public void SaveData()
        {
            XmlSerializer writer =
                new XmlSerializer(typeof(OpenSauce));

            var path = "test.xml";
            FileStream file = File.Create(path);

            writer.Serialize(file, OpenSauce);
            file.Close();
        }
    }
}
