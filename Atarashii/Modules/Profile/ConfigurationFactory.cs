using System;
using System.IO;
using System.Text;
using Atarashii.Modules.Profile.Options;

namespace Atarashii.Modules.Profile
{
    /// <summary>
    ///     Creates Profile Configuration instances.
    /// </summary>
    public static class ConfigurationFactory
    {
        /// <summary>
        ///     Length of the blam.sav binary.
        /// </summary>
        private const int BlamLength = 0x2000;

        /// <summary>
        ///     Data length of the profile name property.
        /// </summary>
        private const int NameOffset = 0x2;

        /// <summary>
        ///     Offset of the profile name property.
        /// </summary>
        private const int NameLength = 0xB;

        /// <summary>
        ///     Data length of the player colour property.
        /// </summary>
        private const int ColourOffset = 0x11a;

        /// <summary>
        ///     Deserialises a given binary stream to a Profile Configuration instance.
        /// </summary>
        /// <param name="stream">
        ///     Binary representation of a serialised Profile Configuration object (blam.sav binary).
        /// </param>
        /// <returns>
        ///     Profile Configuration object instance.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///    Provided stream object length does not match the blam.sav length.
        /// </exception>
        public static Configuration GetFromStream(Stream stream)
        {
            if (stream.Length != BlamLength)
                throw new ArgumentOutOfRangeException(nameof(stream),
                    "Provided stream object length does not match the blam.sav length.");

            var configuration = new Configuration
            {
                // profile name
                Name =
                {
                    Value = new Func<Stream, string>(x =>
                    {
                        var data = new byte[NameLength];

                        stream.Position = NameOffset;

                        for (var i = 0; i < data.Length; i++)
                        {
                            stream.Read(data, i, 1);
                            stream.Position++; // skip null bytes
                        }

                        return Encoding.ASCII.GetString(data);
                    })(stream)
                },

                // player colour
                Colour =
                {
                    Value = new Func<Stream, Colour.Type>(x =>
                    {
                        using (var reader = new BinaryReader(stream))
                        {
                            reader.BaseStream.Seek(ColourOffset, SeekOrigin.Begin);
                            return (Colour.Type) reader.ReadByte();
                        }
                    })(stream)
                }
            };

            return configuration;
        }
    }
}