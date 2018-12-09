using System.IO;
using System.Xml.Serialization;

namespace SPV3.Installer
{
    /// <summary>
    ///     Allows an Installer instance state to be serialised to/serialised from an XML string.
    /// </summary>
    public class Persistence
    {
        /// <summary>
        ///     Serialises the inbound Installer instance to an XML string.
        /// </summary>
        /// <param name="installer">
        ///     Installer instance to serialise.
        /// </param>
        /// <returns>
        ///     XML string representing the inbound Installer state.
        /// </returns>
        public static string ToXml(Installer installer)
        {
            var serializer = new XmlSerializer(typeof(Installer));
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, installer);
                return writer.ToString();
            }
        }

        /// <summary>
        ///     Deserialises the inbound XML string to an Installer instance.
        /// </summary>
        /// <param name="xml">
        ///     XML string representing the inbound Installer state.
        /// </param>
        /// <returns>
        ///     Installer instance representing the inbound XML string.
        /// </returns>
        public static Installer FromXml(string xml)
        {
            var serializer = new XmlSerializer(typeof(Installer));
            using (TextReader reader = new StringReader(xml))
            {
                return (Installer) serializer.Deserialize(reader);
            }
        }
    }
}