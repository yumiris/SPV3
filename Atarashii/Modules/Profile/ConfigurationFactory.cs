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
        ///     Offset of the profile name property.
        /// </summary>
        private const int NameOffset = 0x2;

        /// <summary>
        ///     Data length of the profile name property.
        /// </summary>
        private const int NameLength = 0xB;

        /// <summary>
        ///     Offset of the player colour property.
        /// </summary>
        private const int ColourOffset = 0x11a;

        /// <summary>
        ///     Offset of the horizontal mouse sensitivity property.
        /// </summary>
        private const int MouseSensitivityHorizontalOffset = 0x954;

        /// <summary>
        ///     Offset of the vertical mouse sensitivity property.
        /// </summary>
        private const int MouseSensitivityVerticalOffset = 0x955;

        /// <summary>
        ///     Offset of the mouse vertical axis inversion property.
        /// </summary>
        private const int MouseInvertVerticalAxisOffset = 0x12F;

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
        ///     Provided stream object length does not match the blam.sav length.
        /// </exception>
        public static Configuration GetFromStream(Stream stream)
        {
            if (stream.Length != BlamLength)
                throw new ArgumentOutOfRangeException(nameof(stream),
                    "Provided stream object length does not match the blam.sav length.");

            var reader = new BinaryReader(stream);

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
                    Value = (Colour.Type) GetByte(reader, ColourOffset)
                },

                // mouse
                Mouse =
                {
                    Sensitivity =
                    {
                        Horizontal = GetByte(reader, MouseSensitivityHorizontalOffset),
                        Vertical = GetByte(reader, MouseSensitivityVerticalOffset)
                    },

                    InvertVerticalAxis = GetBool(reader, MouseInvertVerticalAxisOffset)
                }
            };

            reader.Dispose();

            return configuration;
        }

        /// <summary>
        ///     Returns a boolean value from the inbound binary reader at the given offset.
        /// </summary>
        /// <param name="reader">
        ///     Binary reader to retrieve boolean value from.
        /// </param>
        /// <param name="offset">
        ///     Offset of the respective boolean.
        /// </param>
        /// <returns>
        ///     Boolean value.
        /// </returns>
        private static byte GetByte(BinaryReader reader, int offset)
        {
            reader.BaseStream.Seek(offset, SeekOrigin.Begin);
            return reader.ReadByte();
        }

        /// <summary>
        ///     Returns a boolean value from the inbound binary reader at the given offset.
        /// </summary>
        /// <param name="reader">
        ///     Binary reader to retrieve boolean value from.
        /// </param>
        /// <param name="offset">
        ///     Offset of the respective boolean.
        /// </param>
        /// <returns>
        ///     Boolean value.
        /// </returns>
        private static bool GetBool(BinaryReader reader, int offset)
        {
            reader.BaseStream.Seek(offset, SeekOrigin.Begin);
            return reader.ReadBoolean();
        }
    }
}