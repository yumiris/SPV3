using System.Xml.Serialization;

namespace Promise.Library.OpenSauce.HUD
{
    public class HudScale
    {
        [XmlElement(ElementName = "X")]
        public int ScaleX { get; set; } = 1;

        [XmlElement(ElementName = "Y")]
        public int ScaleY { get; set; } = 1;
    }
}