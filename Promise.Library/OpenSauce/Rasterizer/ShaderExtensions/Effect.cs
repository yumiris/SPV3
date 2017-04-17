using System.Xml.Serialization;

namespace Promise.Library.OpenSauce.Rasterizer.ShaderExtensions
{
    public class Effect
    {
        [XmlElement(ElementName = "DepthFade")]
        public bool IsDepthFadeEnabled { get; set; } = true;
    }
}
