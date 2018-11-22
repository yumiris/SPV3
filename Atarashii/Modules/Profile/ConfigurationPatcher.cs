using System;
using System.IO;
using System.Text;

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
            SetBytes(writer, Configuration.NameOffset, new Func<string, byte[]>(x =>
            {
                var result = new byte[Configuration.NameLength * 2];
                var encode = Encoding.ASCII.GetBytes(x);

                var j = 0;
                for (var i = 0; i < x.Length; i++)
                {
                    result[j] = encode[i];
                    j += 2;
                }

                return result;
            })(_configuration.Name.Value));

            // colour
            SetByte(writer, Configuration.ColourOffset, (byte) _configuration.Colour.Value);

            // mouse
            {
                // sensitivity
                {
                    SetByte(writer, Configuration.MouseSensitivityHorizontalOffset,
                        _configuration.Mouse.Sensitivity.Horizontal);

                    SetByte(writer, Configuration.MouseSensitivityVerticalOffset,
                        _configuration.Mouse.Sensitivity.Vertical);
                }

                // axis
                SetBool(writer, Configuration.MouseSensitivityHorizontalOffset,
                    _configuration.Mouse.InvertVerticalAxis);
            }

            // audio
            {
                // volume
                {
                    SetByte(writer, Configuration.AudioVolumeMasterOffset,
                        _configuration.Audio.Volume.Master);

                    SetByte(writer, Configuration.AudioVolumeEffectsOffset,
                        _configuration.Audio.Volume.Effects);

                    SetByte(writer, Configuration.AudioVolumeMusicOffset,
                        _configuration.Audio.Volume.Music);
                }

                // quality
                SetByte(writer, Configuration.AudioQualityOffset,
                    (byte) _configuration.Audio.Quality.Value);

                // variety
                SetByte(writer, Configuration.AudioVarietyOffset,
                    (byte) _configuration.Audio.Variety.Value);
            }

            // video
            {
                // resolution
                {
                    SetShort(writer, Configuration.VideoResolutionWidthOffset,
                        _configuration.Video.Resolution.Width);

                    SetShort(writer, Configuration.VideoResolutionHeightOffset,
                        _configuration.Video.Resolution.Height);
                }

                // frame rate
                SetByte(writer, Configuration.VideoFrameRateOffset,
                    (byte) _configuration.Video.FrameRate.Value);

                // effects
                {
                    SetBool(writer, Configuration.VideoEffectsSpecularOffset,
                        _configuration.Video.Effects.Specular);

                    SetBool(writer, Configuration.VideoEffectsShadowsOffset,
                        _configuration.Video.Effects.Shadows);

                    SetBool(writer, Configuration.VideoEffectsDecalsOffset,
                        _configuration.Video.Effects.Decals);
                }

                // particles
                SetByte(writer, Configuration.VideoParticlesOffset,
                    (byte) _configuration.Video.Particles.Value);

                // quality
                SetByte(writer, Configuration.VideoQualityOffset,
                    (byte) _configuration.Video.Quality.Value);
            }

            // network
            {
                // connection
                SetByte(writer, Configuration.NetworkConnectionTypeOffset,
                    (byte) _configuration.Network.Connection.Value);

                // ports
                {
                    SetShort(writer, Configuration.NetworkPortServerOffset,
                        _configuration.Network.Port.Server);

                    SetShort(writer, Configuration.NetworkPortClientOffset,
                        _configuration.Network.Port.Client);
                }
            }

            writer.Dispose();
        }

        // TODO: Use generic method

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