using System;
using System.IO;
using System.Text;
using static Atarashii.Modules.Profile.Configuration;

namespace Atarashii.Modules.Profile
{
    public class ConfigurationPatcher
    {
        private readonly Configuration _configuration;

        public ConfigurationPatcher(Configuration configuration)
        {
            _configuration = configuration;
        }

        public void PatchTo(Stream stream)
        {
            var writer = new BinaryWriter(stream);

            // name
            SetBytes(writer, NameOffset, new Func<string, byte[]>(x =>
            {
                var result = new byte[NameLength * 2];
                var encode = Encoding.ASCII.GetBytes(x);

                var j = 0;
                for (var i = 0; i < x.Length; i++)
                {
                    result[j] = encode[i];
                    j += 2;
                }

                return result;
            })(_configuration.Name));

            // colour
            SetByte(writer, ColourOffset, (byte) _configuration.Colour);

            // mouse
            {
                // sensitivity
                {
                    SetByte(writer, MouseSensitivityHorizontalOffset,
                        _configuration.Mouse.Sensitivity.Horizontal);

                    SetByte(writer, MouseSensitivityVerticalOffset,
                        _configuration.Mouse.Sensitivity.Vertical);
                }

                // axis
                SetBool(writer, MouseInvertVerticalAxisOffset,
                    _configuration.Mouse.InvertVerticalAxis);
            }

            // audio
            {
                // volume
                {
                    SetByte(writer, AudioVolumeMasterOffset,
                        _configuration.Audio.Volume.Master);

                    SetByte(writer, AudioVolumeEffectsOffset,
                        _configuration.Audio.Volume.Effects);

                    SetByte(writer, AudioVolumeMusicOffset,
                        _configuration.Audio.Volume.Music);
                }

                // quality
                SetByte(writer, AudioQualityOffset,
                    (byte) _configuration.Audio.Quality);

                // variety
                SetByte(writer, AudioVarietyOffset,
                    (byte) _configuration.Audio.Variety);
            }

            // video
            {
                // resolution
                {
                    SetShort(writer, VideoResolutionWidthOffset,
                        _configuration.Video.Resolution.Width);

                    SetShort(writer, VideoResolutionHeightOffset,
                        _configuration.Video.Resolution.Height);
                }

                // frame rate
                SetByte(writer, VideoFrameRateOffset,
                    (byte) _configuration.Video.FrameRate);

                // effects
                {
                    SetBool(writer, VideoEffectsSpecularOffset,
                        _configuration.Video.Effects.Specular);

                    SetBool(writer, VideoEffectsShadowsOffset,
                        _configuration.Video.Effects.Shadows);

                    SetBool(writer, VideoEffectsDecalsOffset,
                        _configuration.Video.Effects.Decals);
                }

                // particles
                SetByte(writer, VideoParticlesOffset,
                    (byte) _configuration.Video.Particles);

                // quality
                SetByte(writer, VideoQualityOffset,
                    (byte) _configuration.Video.Quality);
            }

            // network
            {
                // connection
                SetByte(writer, NetworkConnectionTypeOffset,
                    (byte) _configuration.Network.Connection);

                // ports
                {
                    SetShort(writer, NetworkPortServerOffset,
                        _configuration.Network.Port.Server);

                    SetShort(writer, NetworkPortClientOffset,
                        _configuration.Network.Port.Client);
                }
            }
        }

        private static void SetBytes(BinaryWriter writer, int offset, byte[] data)
        {
            writer.BaseStream.Seek(offset, SeekOrigin.Begin);
            writer.Write(data);
        }

        private static void SetByte(BinaryWriter writer, int offset, byte data)
        {
            writer.BaseStream.Seek(offset, SeekOrigin.Begin);
            writer.Write(data);
        }

        private static void SetBool(BinaryWriter writer, int offset, bool data)
        {
            writer.BaseStream.Seek(offset, SeekOrigin.Begin);
            writer.Write(data);
        }

        private static void SetShort(BinaryWriter writer, int offset, ushort data)
        {
            writer.BaseStream.Seek(offset, SeekOrigin.Begin);
            writer.Write(data);
        }
    }
}