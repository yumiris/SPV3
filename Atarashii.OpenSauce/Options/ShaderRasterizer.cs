using System.Xml.Serialization;

namespace Atarashii.OpenSauce.Options
{
    public class ShaderRasterizer
    {
        [XmlElement(ElementName = "ShaderExtensions")]
        public RasterizerShaderExtensions RasterizerShaderExtensions { get; set; } = new RasterizerShaderExtensions();
    }
}