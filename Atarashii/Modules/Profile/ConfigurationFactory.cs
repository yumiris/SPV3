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
        ///     Deserialises a given binary stream to a Profile Configuration instance.
        /// </summary>
        /// <param name="stream">
        ///     Binary representation of a serialised Profile Configuration object (blam.sav binary).
        /// </param>
        /// <returns>
        ///     Profile Configuration object instance.
        /// </returns>
        public static Configuration GetFromStream(Stream stream)
        {
            throw new NotImplementedException();
        }
    }
}