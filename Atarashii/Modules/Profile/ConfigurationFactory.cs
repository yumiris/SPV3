using System;
using System.IO;

namespace Atarashii.Modules.Profile
{
    /// <summary>
    ///     Creates Profile Configuration instances.
    /// </summary>
    public class ConfigurationFactory
    {
        /// <summary>
        ///     Length of the blam.sav binary.
        /// </summary>
        private const int BlamLength = 0x2000;

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

            throw new NotImplementedException();
        }
    }
}