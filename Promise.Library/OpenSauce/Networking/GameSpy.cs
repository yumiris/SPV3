using System.Xml.Serialization;

namespace Promise.Library.OpenSauce.Networking
{
    public class GameSpy
    {
        [XmlElement(ElementName = "NoUpdateCheck")]
        public bool NoUpdateCheck { get; set; } = true;
    }
}