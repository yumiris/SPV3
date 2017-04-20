using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Promise.Library.Serialisation
{
    public class XmlSerialisation<T>
    {
        public void SerialiseNewXml(T objectToSerialise, string xmlFileName)
        {
            using (FileStream fileStream = File.Create(xmlFileName))
            {
                new XmlSerializer(objectToSerialise.GetType()).Serialize(fileStream, objectToSerialise);
            }
        }

        public T GetDeserialisedInstance(string xmlFileName)
        {
            if (!File.Exists(xmlFileName))
                throw new FileNotFoundException($"Could not find {xmlFileName} configuration file.");

            using (XmlReader xmlReader = XmlReader.Create(xmlFileName))
            {
                return (T) new XmlSerializer(typeof(T)).Deserialize(xmlReader);
            }
        }
    }
}