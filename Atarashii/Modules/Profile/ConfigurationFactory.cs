using System;
using System.IO;

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
        private const int NameLength = 0xB;

        /// <summary>
        ///     Data length of the profile name property.
        /// </summary>
        private const int NameOffset = 0x2;

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

                        return System.Text.Encoding.ASCII.GetString(data);
                    })(stream)
                }
            };

            return configuration;
        }
    }
}