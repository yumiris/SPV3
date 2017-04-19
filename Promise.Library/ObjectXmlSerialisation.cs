using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Promise.Library
{
    public class ObjectXmlSerialisation<T>
    {
        public void SerialiseNewXml(T objectToSerialise, string fileName)
        {
            using (FileStream file = File.Create(fileName))
                new XmlSerializer(objectToSerialise.GetType()).Serialize(file, objectToSerialise);
        }

        public T GetDeserialisedInstance(string configurationFileName)
        { 
            if (!File.Exists(configurationFileName))
                throw new FileNotFoundException($"Could not find {configurationFileName} configuration file.");

            using (XmlReader reader = XmlReader.Create(configurationFileName))
                return (T)new XmlSerializer(typeof(T)).Deserialize(reader);
        }
    }
}