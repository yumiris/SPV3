using System.Xml.Serialization;

namespace Promise.Library.OpenSauce.Rasterizer.PostProcessing
{
    public class MotionBlur
    {
        [XmlElement(ElementName = "Enabled")]
        public bool IsEnabled { get; set; }

        public double BlurAmount { get; set; } = 0.005;
    }
}
