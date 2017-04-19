using System.Xml.Serialization;

namespace Promise.Library.OpenSauce.Rasterizer
{
    public class GBuffer
    {
        [XmlElement(ElementName = "Enabled")]
        public bool IsEnabled { get; set; } = true;
    }
}