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

        /// <summary>
        ///     PackageRepository constructor.
        /// </summary>
        /// <param name="file">
        ///    Source file for saving & loading package state.
        /// </param>
        public PackageRepository(File file)
        {
            _file = file;
        }

        /// <summary>
        ///     Saves the inbound Package state to the provided File.
        /// </summary>
        /// <remarks>
        ///    The data is saved as a DEFLATE-compressed XML binary.
        /// </remarks>
        /// <param name="package">
        ///    Instance of a Package type.
        /// </param>
        public void Save(Package package)
        {
            /**
             * The instance is serialised to an XML string. This allows us to accurately persist the object's state.
             */
            var xml = new Func<Package, string>(x =>
            {
                using (var stringWriter = new StringWriter())
                {
                    var serialiser = new XmlSerializer(typeof(Package));
                    serialiser.Serialize(stringWriter, package);
                    return stringWriter.ToString();
                }
            })(package);

            /**
             * The XMl is DEFLATE-compressed into a byte array that can be saved to the File.
             */
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

        /// <summary>
        ///     Loads the Package state from the provided File.
        /// </summary>
        /// <returns>
        ///    Instance of a Package type.
        /// </returns>
        public Package Load()
        {
            /**
             * We read the bytes back from the File.
             */
            var bin = System.IO.File.ReadAllBytes(_file);

            /**
             * The bytes are decompressed back from DEFLATE to the XML string.
             */
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