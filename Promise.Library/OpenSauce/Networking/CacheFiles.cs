using System.Xml.Serialization;

namespace Promise.Library.OpenSauce.Networking
{
    public class CacheFiles
    {
        [XmlElement(ElementName = "CheckYeloFilesFirst")]
        public bool IsCheckYeloFilesFirst { get; set; } = true;
    }
}