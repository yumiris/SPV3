using System.Xml.Serialization;

namespace Promise.Library.OpenSauce.Rasterizer.PostProcessing
{
    public class AntiAliasing
    {
        [XmlElement(ElementName = "Enabled")]
        public bool IsEnabled { get; set; }
    }
}