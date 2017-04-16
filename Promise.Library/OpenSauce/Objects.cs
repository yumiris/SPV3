using System.Xml.Serialization;

namespace Promise.Library.OpenSauce
{
    public class Objects
    {
        [XmlElement(ElementName = "VehicleRemapperEnabled")]
        public bool IsVehicleRemapperEnabled { get; set; } = true;
    }
}