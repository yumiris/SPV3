using System.Xml.Serialization;

namespace Promise.Library.OpenSauce.Rasterizer.PostProcessing
{
    public class ExternalEffects
    {
        [XmlElement(ElementName = "Enabled")]
        public bool IsEnabled { get; set; } = true;
    }
}