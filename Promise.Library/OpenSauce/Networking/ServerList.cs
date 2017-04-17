using System.Xml.Serialization;

namespace Promise.Library.OpenSauce.Networking
{
    public class ServerList
    {
        public int Version { get; set; } = 1;

        [XmlElement(ElementName = "Server")]
        public string HaloVersionUrl { get; set; } = "http://os.halomods.com/Halo1/CE/Halo1_CE_Version.xml";
    }
}