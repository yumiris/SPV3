using System.IO;
using System.IO.Compression;
using System.Text;
using System.Xml.Serialization;

namespace SPV3.Installer
{
    /// <summary>
    ///     Allows an Installer instance state to be serialised to/serialised from an XML string.
    /// </summary>
    public static class Persistence
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
        ///     Encodes the inbound installer instance into a compressed binary.
        /// </summary>
        /// <param name="installer">
        ///     Installer instance to encode into a compressed binary.
        /// </param>
        /// <returns>
        ///     Byte array representing the inbound Installer instance as a compressed binary.
        /// </returns>
        public static byte[] ToBin(Installer installer)
        {
            using (var outputStream = new MemoryStream())
            using (var bufferStream = new MemoryStream(Encoding.UTF8.GetBytes(ToXml(installer))))
            using (var zippedStream = new DeflateStream(outputStream, CompressionMode.Compress, false))
            {
                bufferStream.CopyTo(zippedStream);
                zippedStream.Close();
                return outputStream.ToArray();
            }
        }

        /// <summary>
        ///     Deserialises the inbound XML string to an Installer instance.
        /// </summary>
        /// <param name="xml">
        ///     XML string representing an Installer state.
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

        /// <summary>
        ///     Decodes the inbound compressed binary data to an Installer instance.
        /// </summary>
        /// <param name="data">
        ///     Compressed data representing an Installer state.
        /// </param>
        /// <returns>
        ///     Installer instance representing the inbound data.
        /// </returns>
        public static Installer FromBin(byte[] data)
        {
            using (var outputStream = new MemoryStream())
            using (var bufferStream = new MemoryStream(data))
            using (var zippedStream = new DeflateStream(bufferStream, CompressionMode.Decompress))
            {
                zippedStream.CopyTo(outputStream);
                zippedStream.Close();
                return FromXml(Encoding.ASCII.GetString(outputStream.ToArray()));
            }
        }
    }
}