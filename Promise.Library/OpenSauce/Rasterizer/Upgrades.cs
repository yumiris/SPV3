using System.Xml.Serialization;

namespace Promise.Library.OpenSauce.Rasterizer
{
    public class Upgrades
    {
        [XmlElement(ElementName = "MaximumRenderedTriangles")]
        public bool IsMaximumRenderedTrianglesEnabled { get; set; } = true;
    }
}