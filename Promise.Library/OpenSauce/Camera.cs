using System.Xml.Serialization;

namespace Promise.Library.OpenSauce
{
    public class Camera
    {
        public int FieldOfView { get; set; } = 85;

        [XmlElement(ElementName = "IgnoreFOVChangeInCinematics")]
        public bool IgnoreFovChangeInCinematics { get; set; } = true;

        [XmlElement(ElementName = "IgnoreFOVChangeInMainMenu")]
        public bool IgnoreFovChangeInMainMenu { get; set; } = true;
    }
}