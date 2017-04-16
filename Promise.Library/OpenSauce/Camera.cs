using System.Xml.Serialization;

namespace Promise.Library.OpenSauce
{
    public class Camera
    {
        public int FieldOfView { get; set; } = 70;

        [XmlElement(ElementName = "IgnoreFOVChangeInCinematics")]
        public bool IgnoreFovChangeInCinematics { get; set; } = true;

        [XmlElement(ElementName = "IgnoreFOVChangeInMainMenu")]
        public bool IgnoreFovChangeInMainMenu { get; set; } = true;
    }
}
