using System.Xml.Serialization;

namespace Promise.Library.OpenSauce.Rasterizer.PostProcessing
{
    public class MapEffects
    {
        [XmlElement(ElementName = "Enabled")]
        public bool IsEnabled { get; set; } = true;
    }
}
