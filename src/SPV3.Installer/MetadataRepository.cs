using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Xml.Serialization;
using File = SPV3.Domain.File;

namespace SPV3.Installer
{
    public class MetadataRepository
    {
        /// <summary>
        ///     Default binary File name.
        /// </summary>
        public const string Binary = "0x00.bin";

        private readonly File _file;

        /// <summary>
        ///     MetadataRepository constructor.
        /// </summary>
        /// <param name="file">
        ///     Source file for saving & loading metadata state.
        /// </param>
        public MetadataRepository(File file)
        {
            _file = file ?? (File) Binary;
        }

        /// <summary>
        ///     Saves the inbound Metadata state to the provided File.
        /// </summary>
        /// <remarks>
        ///     The data is saved as a DEFLATE-compressed XML binary.
        /// </remarks>
        /// <param name="metadata">
        ///     Instance of a Metadata type.
        /// </param>
        public void Save(Metadata metadata)
        {
            /**
             * The instance is serialised to an XML string. This allows us to accurately persist the object's state.
             */
            var xml = new Func<Metadata, string>(x =>
            {
                using (var stringWriter = new StringWriter())
                {
                    var serialiser = new XmlSerializer(typeof(Metadata));
                    serialiser.Serialize(stringWriter, metadata);
                    return stringWriter.ToString();
                }
            })(metadata);

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
        ///     Loads the Metadata state from the provided File.
        /// </summary>
        /// <returns>
        ///     Instance of a Metadata type.
        /// </returns>
        public Metadata Load()
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

            var serializer = new XmlSerializer(typeof(Metadata));
            using (TextReader reader = new StringReader(xml))
            {
                return (Metadata) serializer.Deserialize(reader);
            }
        }
    }
}