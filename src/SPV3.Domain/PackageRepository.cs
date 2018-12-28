using System.IO;
using System.IO.Compression;
using System.Text;
using System.Xml.Serialization;

namespace SPV3.Domain
{
    public class PackageRepository
    {
        private string _source;

        public PackageRepository(string source)
        {
            _source = source;
        }

        public void Save(Package package)
        {
            File.WriteAllBytes(_source, ToBin(package));
        }

        public Package Load()
        {
            return FromBin(File.ReadAllBytes(_source));
        }

        private byte[] ToBin(Package package)
        {
            using (var outputStream = new MemoryStream())
            using (var bufferStream = new MemoryStream(Encoding.UTF8.GetBytes(ToXml(package))))
            using (var zippedStream = new DeflateStream(outputStream, CompressionMode.Compress, false))
            {
                bufferStream.CopyTo(zippedStream);
                zippedStream.Close();
                return outputStream.ToArray();
            }
        }

        private Package FromBin(byte[] data)
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

        private string ToXml(Package package)
        {
            var serializer = new XmlSerializer(typeof(Package));
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, package);
                return writer.ToString();
            }
        }

        private Package FromXml(string xml)
        {
            var serializer = new XmlSerializer(typeof(Package));
            using (TextReader reader = new StringReader(xml))
            {
                return (Package) serializer.Deserialize(reader);
            }
        }
    }
}