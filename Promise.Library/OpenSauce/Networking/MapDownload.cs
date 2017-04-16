using System.Xml.Serialization;

namespace Promise.Library.OpenSauce.Networking
{
    public class MapDownload
    {
        [XmlElement(ElementName = "Enabled")]
        public bool IsEnabled { get; set; } = true;
    }
}