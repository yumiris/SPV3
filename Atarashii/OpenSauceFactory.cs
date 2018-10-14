using System.IO;
using System.Xml.Serialization;

namespace Atarashii
{
    /// <summary>
    ///     Creates OpenSauce instances.
    /// </summary>
    public class OpenSauceFactory
    {
        /// <summary>
        ///     Deserialises a given XML string to an OpenSauce instance.
        /// </summary>
        /// <param name="xml">
        ///     XML representation of a serialised OpenSauce object.
        /// </param>
        /// <returns>
        ///     OpenSauce object instance.
        /// </returns>
        public static OpenSauce GetFromXml(string xml)
        {
            var stringReader = new StringReader(xml);
            var serializer = new XmlSerializer(typeof(OpenSauce));
            return serializer.Deserialize(stringReader) as OpenSauce;
        }
    }
}