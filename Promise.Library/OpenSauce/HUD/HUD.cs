using System.Xml.Serialization;

namespace Promise.Library.OpenSauce.HUD
{
    public class Hud
    {
        [XmlElement(ElementName = "ShowHUD")]
        public bool ShowHud { get; set; } = true;

        [XmlElement(ElementName = "ScaleHUD")]
        public bool ScaleHud { get; set; } = true;

        [XmlElement(ElementName = "HUDScale")]
        public HudScale HudScale { get; set; } = new HudScale();
    }
}