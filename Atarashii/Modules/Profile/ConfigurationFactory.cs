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
        ///     Offset of the audio master volume property.
        /// </summary>
        private const int AudioVolumeMasterOffset = 0xB78;

        /// <summary>
        ///     Offset of the audio master volume property.
        /// </summary>
        private const int AudioVolumeEffectsOffset = 0xB79;

        /// <summary>
        ///     Offset of the audio master volume property.
        /// </summary>
        private const int AudioVolumeMusicOffset = 0xB7A;

        /// <summary>
        ///     Offset of the audio quality property.
        /// </summary>
        private const int AudioQualityOffset = 0xB7D;

        /// <summary>
        ///     Offset of the audio variety property.
        /// </summary>
        private const int AudioVarietyOffset = 0xB7F;

        /// <summary>
        ///     Offset of the video resolution width property.
        /// </summary>
        private const int VideoResolutionWidthOffset = 0xA68;

        /// <summary>
        ///     Offset of the video resolution height property.
        /// </summary>
        private const int VideoResolutionHeightOffset = 0xA6A;

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

                Colour =
                {
                    Value = (Colour.Type) GetByte(reader, ColourOffset)
                },

                Mouse =
                {
                    Sensitivity =
                    {
                        Horizontal = GetByte(reader, MouseSensitivityHorizontalOffset),
                        Vertical = GetByte(reader, MouseSensitivityVerticalOffset)
                    },

                    InvertVerticalAxis = GetBool(reader, MouseInvertVerticalAxisOffset)
                },

                Audio =
                {
                    Volume =
                    {
                        Master = GetByte(reader, AudioVolumeMasterOffset),
                        Effects = GetByte(reader, AudioVolumeEffectsOffset),
                        Music = GetByte(reader, AudioVolumeMusicOffset)
                    },

                    Quality =
                    {
                        Value = (Quality.Type) GetByte(reader, AudioQualityOffset)
                    },

                    Variety =
                    {
                        Value = (Quality.Type) GetByte(reader, AudioVarietyOffset)
                    }
                },
                
                Video =
                {
                    Resolution =
                    {
                        Width = GetShort(reader, VideoResolutionWidthOffset),
                        Height = GetShort(reader, VideoResolutionHeightOffset)
                    }
                }
            };

            reader.Dispose();

            return configuration;
        }

        /// <summary>
        ///     Returns a byte value from the inbound binary reader at the given offset.
        /// </summary>
        /// <param name="reader">
        ///     Binary reader to retrieve byte value from.
        /// </param>
        /// <param name="offset">
        ///     Offset of the respective byte.
        /// </param>
        /// <returns>
        ///     byte value.
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

        /// <summary>
        ///     Returns a unsigned short value from the inbound binary reader at the given offset.
        /// </summary>
        /// <param name="reader">
        ///     Binary reader to retrieve unsigned short value from.
        /// </param>
        /// <param name="offset">
        ///     Offset of the respective unsigned short.
        /// </param>
        /// <returns>
        ///     Unsigned short value.
        /// </returns>
        private static ushort GetShort(BinaryReader reader, int offset)
        {
            reader.BaseStream.Seek(offset, SeekOrigin.Begin);
            return reader.ReadUInt16();
        }
    }
}