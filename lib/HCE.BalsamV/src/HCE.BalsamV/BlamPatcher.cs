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

namespace HCE.BalsamV
{
    /// <summary>
    ///     Patches a blam.sav binary with Blam-type property values.
    /// </summary>
    public class BlamPatcher
    {
        /// <summary>
        ///     Blam configuration.
        /// </summary>
        private readonly Blam _blam;

        /// <summary>
        ///     BlamPatcher constructor.
        /// </summary>
        /// <param name="blam">
        ///     <see cref="_blam" />
        /// </param>
        public BlamPatcher(Blam blam)
        {
            _blam = blam;
        }

        /// <summary>
        ///     Patches the Blam object property data to the given blam.sav binary path.
        /// </summary>
        /// <param name="path">
        ///     Absolute path to the blam.sav binary file.
        /// </param>
        public void PatchToBinary(string path)
        {
            using (var ms = new MemoryStream())
            using (var fs = File.Open(path, FileMode.Open))
            {
                fs.CopyTo(ms);

                new BlamPatcher(_blam).PatchToStream(ms);
                new BlamForger().Forge(ms);

                ms.Position = 0;
                fs.Position = 0;

                ms.CopyTo(fs);
            }
        }

        /// <summary>
        ///     Assigns the Blam values to the inbound blam.sav-representing Stream object.
        /// </summary>
        /// <param name="stream">
        ///     Stream containing blam.sav binary data.
        /// </param>
        public void PatchToStream(Stream stream)
        {
            var writer = new BinaryWriter(stream);

            // name
            SetBytes(writer, Blam.NameOffset, new Func<string, byte[]>(x =>
            {
                var result = new byte[Blam.NameLength * 2];
                var encode = Encoding.ASCII.GetBytes(x);

                var j = 0;
                for (var i = 0; i < x.Length; i++)
                {
                    result[j] = encode[i];
                    j += 2;
                }

                return result;
            })(_blam.Name));

            // colour
            SetByte(writer, Blam.ColourOffset, (byte) _blam.Colour);

            // mouse
            {
                // sensitivity
                {
                    SetByte(writer, Blam.MouseSensitivityHorizontalOffset,
                        _blam.Mouse.Sensitivity.Horizontal);

                    SetByte(writer, Blam.MouseSensitivityVerticalOffset,
                        _blam.Mouse.Sensitivity.Vertical);
                }

                // axis
                SetBool(writer, Blam.MouseInvertVerticalAxisOffset,
                    _blam.Mouse.InvertVerticalAxis);
            }

            // audio
            {
                // volume
                {
                    SetByte(writer, Blam.AudioVolumeMasterOffset,
                        _blam.Audio.Volume.Master);

                    SetByte(writer, Blam.AudioVolumeEffectsOffset,
                        _blam.Audio.Volume.Effects);

                    SetByte(writer, Blam.AudioVolumeMusicOffset,
                        _blam.Audio.Volume.Music);
                }

                // quality
                SetByte(writer, Blam.AudioQualityOffset,
                    (byte) _blam.Audio.Quality);

                // variety
                SetByte(writer, Blam.AudioVarietyOffset,
                    (byte) _blam.Audio.Variety);
            }

            // video
            {
                // resolution
                {
                    SetUShort(writer, Blam.VideoResolutionWidthOffset,
                        _blam.Video.Resolution.Width);

                    SetUShort(writer, Blam.VideoResolutionHeightOffset,
                        _blam.Video.Resolution.Height);
                }

                // frame rate
                SetByte(writer, Blam.VideoFrameRateOffset,
                    (byte) _blam.Video.FrameRate);

                // effects
                {
                    SetBool(writer, Blam.VideoEffectsSpecularOffset,
                        _blam.Video.Effects.Specular);

                    SetBool(writer, Blam.VideoEffectsShadowsOffset,
                        _blam.Video.Effects.Shadows);

                    SetBool(writer, Blam.VideoEffectsDecalsOffset,
                        _blam.Video.Effects.Decals);
                }

                // particles
                SetByte(writer, Blam.VideoParticlesOffset,
                    (byte) _blam.Video.Particles);

                // quality
                SetByte(writer, Blam.VideoQualityOffset,
                    (byte) _blam.Video.Quality);
            }

            // network
            {
                // connection
                SetByte(writer, Blam.NetworkConnectionTypeOffset,
                    (byte) _blam.Network.Connection);

                // ports
                {
                    SetUShort(writer, Blam.NetworkPortServerOffset,
                        _blam.Network.Port.Server);

                    SetUShort(writer, Blam.NetworkPortClientOffset,
                        _blam.Network.Port.Client);
                }
            }
        }

        /// <summary>
        ///     Writes byte arrays using the inbound binary writer at the specified offset.
        /// </summary>
        /// <param name="writer">
        ///     BinaryWriter used for writing the inbound data at the specified offset.
        /// </param>
        /// <param name="offset">
        ///     Offset to write the inbound data at.
        /// </param>
        /// <param name="data">
        ///     Data to write at the specified offset.
        /// </param>
        private static void SetBytes(BinaryWriter writer, int offset, byte[] data)
        {
            writer.BaseStream.Seek(offset, SeekOrigin.Begin);
            writer.Write(data);
        }

        /// <summary>
        ///     Writes an unsigned byte using the inbound binary writer at the specified offset.
        /// </summary>
        /// <param name="writer">
        ///     BinaryWriter used for writing the inbound data at the specified offset.
        /// </param>
        /// <param name="offset">
        ///     Offset to write the inbound data at.
        /// </param>
        /// <param name="data">
        ///     Data to write at the specified offset.
        /// </param>
        private static void SetByte(BinaryWriter writer, int offset, byte data)
        {
            writer.BaseStream.Seek(offset, SeekOrigin.Begin);
            writer.Write(data);
        }

        /// <summary>
        ///     Writes a boolean value using the inbound binary writer at the specified offset.
        /// </summary>
        /// <param name="writer">
        ///     BinaryWriter used for writing the inbound data at the specified offset.
        /// </param>
        /// <param name="offset">
        ///     Offset to write the inbound data at.
        /// </param>
        /// <param name="data">
        ///     Data to write at the specified offset.
        /// </param>
        private static void SetBool(BinaryWriter writer, int offset, bool data)
        {
            writer.BaseStream.Seek(offset, SeekOrigin.Begin);
            writer.Write(data);
        }

        /// <summary>
        ///     Writes an unsigned short value using the inbound binary writer at the specified offset.
        /// </summary>
        /// <param name="writer">
        ///     BinaryWriter used for writing the inbound data at the specified offset.
        /// </param>
        /// <param name="offset">
        ///     Offset to write the inbound data at.
        /// </param>
        /// <param name="data">
        ///     Data to write at the specified offset.
        /// </param>
        private static void SetUShort(BinaryWriter writer, int offset, ushort data)
        {
            writer.BaseStream.Seek(offset, SeekOrigin.Begin);
            writer.Write(data);
        }
    }
}