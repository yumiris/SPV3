using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Atarashii
{
    /// <summary>
    ///     Creates OpenSauce instances.
    /// </summary>
    public class OpenSauceFactory
    {
        /// <summary>
        ///     Deserialises a given XML string to an OpenSauce instance.
        /// </summary>
        /// <param name="xml">
        ///     XML representation of a serialised OpenSauce object.
        /// </param>
        /// <returns>
        ///     OpenSauce object instance.
        /// </returns>
        public static OpenSauce GetFromXml(string xml)
        {
            var stringReader = new StringReader(xml);
            var serializer = new XmlSerializer(typeof(OpenSauce));
            return serializer.Deserialize(stringReader) as OpenSauce;
        }

        /// <summary>
        ///     Builds a list of packages that represent the OpenSauce installation data.
        /// </summary>
        /// <param name="hcePath">
        ///     The HCE directory path -- used to install the OpenSauce library data to.
        /// </param>
        /// <returns>
        ///     A list of OpenSauce packages that replicate an original OS installation when installed.
        /// </returns>
        public static List<Package> GetPackages(string hcePath)
        {
            var guiDirPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            var usrDirPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

            return new List<Package>
            {
                new Package("lib.pkg", "OpenSauce core and dependencies", hcePath),
                new Package("gui.pkg", "In-game OpenSauce UI assets", guiDirPath),
                new Package("usr.pkg", "OpenSauce XML user configuration", usrDirPath)
            };
        }
    }
}