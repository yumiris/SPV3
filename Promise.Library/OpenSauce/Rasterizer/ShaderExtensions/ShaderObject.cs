using System.Xml.Serialization;

namespace Promise.Library.OpenSauce.Rasterizer.ShaderExtensions
{
    public class ShaderObject
    {
        [XmlElement(ElementName = "NormalMaps")]
        public bool IsNormalMaps { get; set; } = true;

        [XmlElement(ElementName = "DetailNormalMaps")]
        public bool IsDetailNormalMaps { get; set; } = true;

        [XmlElement(ElementName = "SpecularMaps")]
        public bool IsSpecularMaps { get; set; } = true;

        [XmlElement(ElementName = "SpecularLighting")]
        public bool IsSpecularLighting { get; set; } = true;
    }
}