using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Xml.Serialization;

namespace SPV3.Domain
{
    /// <summary>
    ///     Saves and loads Package-type states from the given file.
    /// </summary>
    public class PackageRepository
    {
        private File _file;

        public PackageRepository(File file)
        {
            _file = file;
        }

        public void Save(Package package)
        {
            var xml = new Func<Package, string>(x =>
            {
                using (var stringWriter = new StringWriter())
                {
                    var serialiser = new XmlSerializer(typeof(Package));
                    serialiser.Serialize(stringWriter, package);
                    return stringWriter.ToString();
                }
            })(package);

            var bin = new Func<string, byte[]>(x =>
            {
                using (var outputStream = new MemoryStream())
                using (var bufferStream = new MemoryStream(Encoding.UTF8.GetBytes(x)))
                using (var zippedStream = new DeflateStream(outputStream, CompressionMode.Compress, false))
                {
                    bufferStream.CopyTo(zippedStream);
                    zippedStream.Close();
                    return outputStream.ToArray();
                }
            })(xml);

            System.IO.File.WriteAllBytes(_file, bin);
        }

        public Package Load()
        {
            var bin = System.IO.File.ReadAllBytes(_file);

            var xml = new Func<byte[], string>(x =>
            {
                using (var outputStream = new MemoryStream())
                using (var bufferStream = new MemoryStream(x))
                using (var zippedStream = new DeflateStream(bufferStream, CompressionMode.Decompress))
                {
                    zippedStream.CopyTo(outputStream);
                    zippedStream.Close();
                    return Encoding.ASCII.GetString(outputStream.ToArray());
                }
            })(bin);

            var serializer = new XmlSerializer(typeof(Package));
            using (TextReader reader = new StringReader(xml))
            {
                return (Package) serializer.Deserialize(reader);
            }
        }
    }
}