using System;
using System.IO;
using System.Text;
using Atarashii.Modules.Profile.Options;
using static Atarashii.Modules.Profile.Configuration;

namespace Atarashii.Modules.Profile
{
    /// <summary>
    ///     Creates Profile Configuration instances.
    /// </summary>
    public static class ConfigurationFactory
    {
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

                        return Encoding.ASCII.GetString(data).TrimEnd('\0');
                    })(stream)
                },

                Colour =
                {
                    Value = new Func<BinaryReader, Colour.Type>(x =>
                    {
                        var colour = GetByte(x, ColourOffset);
                        return colour == 0xFF ? Colour.Type.White : (Colour.Type) colour;
                    })(reader)
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
                    },

                    FrameRate =
                    {
                        Value = (FrameRate.Type) GetByte(reader, VideoFrameRateOffset)
                    },

                    Effects =
                    {
                        Specular = GetBool(reader, VideoEffectsSpecularOffset),
                        Shadows = GetBool(reader, VideoEffectsShadowsOffset),
                        Decals = GetBool(reader, VideoEffectsDecalsOffset)
                    },

                    Particles =
                    {
                        Value = (Particles.Type) GetByte(reader, VideoParticlesOffset)
                    },

                    Quality =
                    {
                        Value = (Quality.Type) GetByte(reader, VideoQualityOffset)
                    }
                },

                Network =
                {
                    Connection =
                    {
                        Value = (Connection.Type) GetByte(reader, NetworkConnectionTypeOffset)
                    },

                    Port =
                    {
                        Server = GetShort(reader, NetworkPortServerOffset),
                        Client = GetShort(reader, NetworkPortClientOffset)
                    }
                }
            };

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
        ///     Sbyte value.
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