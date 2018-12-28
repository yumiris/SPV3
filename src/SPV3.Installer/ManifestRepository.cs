using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Xml.Serialization;
using File = SPV3.Domain.File;

namespace SPV3.Installer
{
    public class ManifestRepository
    {
        /// <summary>
        ///     Default binary File name.
        /// </summary>
        public const string Binary = "0x00.bin";

        private readonly File _file;

        /// <summary>
        ///     ManifestRepository constructor.
        /// </summary>
        /// <param name="file">
        ///     Source file for saving & loading manifest state.
        /// </param>
        public ManifestRepository(File file)
        {
            _file = file ?? (File) Binary;
        }

        /// <summary>
        ///     Saves the inbound Manifest state to the provided File.
        /// </summary>
        /// <remarks>
        ///     The data is saved as a DEFLATE-compressed XML binary.
        /// </remarks>
        /// <param name="manifest">
        ///     Instance of a Manifest type.
        /// </param>
        public void Save(Manifest manifest)
        {
            /**
             * The instance is serialised to an XML string. This allows us to accurately persist the object's state.
             */
            var xml = new Func<Manifest, string>(x =>
            {
                using (var stringWriter = new StringWriter())
                {
                    var serialiser = new XmlSerializer(typeof(Manifest));
                    serialiser.Serialize(stringWriter, manifest);
                    return stringWriter.ToString();
                }
            })(manifest);

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
        ///     Loads the Manifest state from the provided File.
        /// </summary>
        /// <returns>
        ///     Instance of a Manifest type.
        /// </returns>
        public Manifest Load()
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

            var serializer = new XmlSerializer(typeof(Manifest));
            using (TextReader reader = new StringReader(xml))
            {
                return (Manifest) serializer.Deserialize(reader);
            }
        }
    }
}