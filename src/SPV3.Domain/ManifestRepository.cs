/**
 * Copyright (C) 2019 Emilian Roman
 * 
 * This file is part of SPV3.Domain.
 * 
 * SPV3.Domain is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * SPV3.Domain is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with SPV3.Domain.  If not, see <http://www.gnu.org/licenses/>.
 */

using System.IO;
using System.IO.Compression;
using System.Text;
using System.Xml.Serialization;

namespace SPV3.Domain
{
    public class ManifestRepository
    {
        /// <summary>
        ///     Default binary File name.
        /// </summary>
        public const string Binary = "0x00.bin";

        /// <summary>
        ///     Source file for saving & loading manifest state.
        /// </summary>
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
            byte[] Deflate(byte[] inflatedBytes)
            {
                using (var deflatedStream = new MemoryStream())
                using (var inflatedStream = new MemoryStream(inflatedBytes))
                using (var compressStream = new DeflateStream(deflatedStream, CompressionMode.Compress))
                {
                    inflatedStream.CopyTo(compressStream);
                    compressStream.Close();
                    return deflatedStream.ToArray();
                }
            }

            string xml;
            using (var stringWriter = new StringWriter())
            {
                var serialiser = new XmlSerializer(typeof(Manifest));
                serialiser.Serialize(stringWriter, manifest);
                xml = stringWriter.ToString();
            }

            var stringAsUtf8 = Encoding.UTF8.GetBytes(xml);
            var deflatedData = Deflate(stringAsUtf8);

            System.IO.File.WriteAllBytes(_file, deflatedData);
        }

        /// <summary>
        ///     Loads the Manifest state from the provided File.
        /// </summary>
        /// <remarks>
        ///     This method expects a DEFLATE-compressed XML binary.
        /// </remarks>
        /// <returns>
        ///     Instance of a Manifest type.
        /// </returns>
        public Manifest Load()
        {
            byte[] Inflate(byte[] deflatedBytes)
            {
                using (var inflatedStream = new MemoryStream())
                using (var deflatedStream = new MemoryStream(deflatedBytes))
                using (var compressStream = new DeflateStream(deflatedStream, CompressionMode.Decompress))
                {
                    compressStream.CopyTo(inflatedStream);
                    compressStream.Close();
                    return inflatedStream.ToArray();
                }
            }

            var deflatedData = System.IO.File.ReadAllBytes(_file);
            var inflatedData = Inflate(deflatedData);
            var utf8AsString = Encoding.UTF8.GetString(inflatedData);

            var serializer = new XmlSerializer(typeof(Manifest));
            using (var reader = new StringReader(utf8AsString))
            {
                return (Manifest) serializer.Deserialize(reader);
            }
        }

        /// <summary>
        ///     Returns the default manifest.
        /// </summary>
        public static Manifest LoadDefault()
        {
            return new ManifestRepository((File) Manifest.Name.Value).Load();
        }
    }
}
