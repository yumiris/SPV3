using System.Xml.Serialization;
using Promise.Library.OpenSauce.Networking;

namespace Promise.Library.OpenSauce
{
    public class OpenSauce
    {
        public CacheFiles CacheFiles { get; set; } = new CacheFiles();
        public Rasterizer.Rasterizer Rasterizer { get; set; } = new Rasterizer.Rasterizer();
        public Camera Camera { get; set; } = new Camera();
        public Networking.Networking Networking { get; set; } = new Networking.Networking();
        public Objects Objects { get; set; } = new Objects();

        [XmlElement(ElementName = "HUD")]
        public HUD.Hud Hud { get; set; } = new HUD.Hud();
    }
}
