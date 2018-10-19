using System.Xml.Serialization;

namespace Atarashii.OpenSauce.Options
{
    public class Camera
    {
        public double FieldOfView { get; set; } = 70.0;

        [XmlElement(ElementName = "IgnoreFOVChangeInCinematics")]
        public bool IgnoreFovChangeInCinematics { get; set; } = true;

        [XmlElement(ElementName = "IgnoreFOVChangeInMainMenu")]
        public bool IgnoreFovChangeInMainMenu { get; set; } = true;
    }
}