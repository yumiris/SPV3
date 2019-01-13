/**
 * Copyright (C) 2018-2019 Emilian Roman
 * 
 * This file is part of HCE.HCE.BalsamV.
 * 
 * HCE.HCE.BalsamV is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * HCE.HCE.BalsamV is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with HCE.HCE.BalsamV.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.IO;
using System.Text;
using HCE.BalsamV.Profile;
using HCE.BalsamV.Settings;

namespace HCE.BalsamV
{
    /// <summary>
    ///     Creates Blam instances.
    /// </summary>
    public static class BlamFactory
    {
        /// <summary>
        ///     Attempts to deserialise a Blam object by detecting a blam.sav on the file system.
        /// </summary>
        /// <returns>
        ///     Blam instance representing a successfully detected blam.sav binary.
        /// </returns>
        /// <exception cref="FileNotFoundException">
        ///     Attempted to detect a blam.sav file and none has been found on the file system.
        /// </exception>
        public static Blam GetFromSystem()
        {
            var name = LastprofFactory.DetectOnSystem().Name;
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                "My Games", "Halo CE", "savegames", name, "blam.sav");

            return GetFromBinary(path);
        }

        /// <summary>
        ///     Attempts to deserialise a Blam object by reading the data of the specified blam.sav binary.
        /// </summary>
        /// <param name="path">
        ///     Path to the blam.sav binary file.
        /// </param>
        /// <returns>
        ///     Configuration instance representing a successfully parsed blam.sav binary.
        /// </returns>
        /// <exception cref="FileNotFoundException">
        ///     Inbound blam.sav path not found.
        /// </exception>
        public static Blam GetFromBinary(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException("Inbound blam.sav path not found.");

            using (var fs = File.Open(path, FileMode.Open))
            {
                return GetFromStream(fs);
            }
        }

        /// <summary>
        ///     Deserialises a given binary stream to a Blam instance.
        /// </summary>
        /// <param name="stream">
        ///     Binary representation of a blam.sav binary.
        /// </param>
        /// <returns>
        ///     Blam object instance.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Provided stream object length does not match the blam.sav length.
        /// </exception>
        public static Blam GetFromStream(Stream stream)
        {
            if (stream.Length != Blam.BlamLength)
                throw new ArgumentOutOfRangeException(nameof(stream),
                    "Provided stream object length does not match the blam.sav length.");

            var reader = new BinaryReader(stream);

            return new Blam
            {
                Name = new Func<Stream, string>(x =>
                {
                    var data = new byte[Blam.NameLength];

                    stream.Position = Blam.NameOffset;

                    for (var i = 0; i < data.Length; i++)
                    {
                        stream.Read(data, i, 1);
                        stream.Position++; // skip null bytes
                    }

                    return Encoding.ASCII.GetString(data).TrimEnd('\0');
                })(stream),

                Colour = new Func<BinaryReader, Colour>(x =>
                {
                    var colour = GetByte(x, Blam.ColourOffset);
                    return colour == 0xFF ? Colour.White : (Colour) colour;
                })(reader),

                Mouse =
                {
                    Sensitivity =
                    {
                        Horizontal = GetByte(reader, Blam.MouseSensitivityHorizontalOffset),
                        Vertical = GetByte(reader, Blam.MouseSensitivityVerticalOffset)
                    },

                    InvertVerticalAxis = GetBool(reader, Blam.MouseInvertVerticalAxisOffset)
                },

                Audio =
                {
                    Volume =
                    {
                        Master = GetByte(reader, Blam.AudioVolumeMasterOffset),
                        Effects = GetByte(reader, Blam.AudioVolumeEffectsOffset),
                        Music = GetByte(reader, Blam.AudioVolumeMusicOffset)
                    },

                    Quality = (Quality) GetByte(reader, Blam.AudioQualityOffset),
                    Variety = (Quality) GetByte(reader, Blam.AudioVarietyOffset)
                },

                Video =
                {
                    Resolution =
                    {
                        Width = GetUShort(reader, Blam.VideoResolutionWidthOffset),
                        Height = GetUShort(reader, Blam.VideoResolutionHeightOffset)
                    },

                    FrameRate = (FrameRate) GetByte(reader, Blam.VideoFrameRateOffset),

                    Effects =
                    {
                        Specular = GetBool(reader, Blam.VideoEffectsSpecularOffset),
                        Shadows = GetBool(reader, Blam.VideoEffectsShadowsOffset),
                        Decals = GetBool(reader, Blam.VideoEffectsDecalsOffset)
                    },

                    Particles = (Particles) GetByte(reader, Blam.VideoParticlesOffset),
                    Quality = (Quality) GetByte(reader, Blam.VideoQualityOffset)
                },

                Network =
                {
                    Connection = (Connection) GetByte(reader, Blam.NetworkConnectionTypeOffset),

                    Port =
                    {
                        Server = GetUShort(reader, Blam.NetworkPortServerOffset),
                        Client = GetUShort(reader, Blam.NetworkPortClientOffset)
                    }
                }
            };
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
        private static ushort GetUShort(BinaryReader reader, int offset)
        {
            reader.BaseStream.Seek(offset, SeekOrigin.Begin);
            return reader.ReadUInt16();
        }
    }
}