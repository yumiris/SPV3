using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Promise.Library.Halo
{
    public class HaloConfiguration
    {
        private const string ConfigurationFileName = "Halo_Settings.User.xml";
        public Halo Halo { get; set; } = new Halo();

        public void Serialise()
        {
            using (var file = File.Create(ConfigurationFileName))
            {
                var writer = new XmlSerializer(typeof(Halo));
                writer.Serialize(file, Halo);
            }
        }

        public Halo GetDeserialisedHalo()
        {
            var xmlSerializer = new XmlSerializer(typeof(Halo));
            Halo deserialisedHalo;

            if (!File.Exists(ConfigurationFileName))
            {
                Halo = new Halo();
                Serialise();
            }

            using (var reader = XmlReader.Create(ConfigurationFileName))
            {
                deserialisedHalo = (Halo) xmlSerializer.Deserialize(reader);
            }

            return deserialisedHalo;
        }
    }
}