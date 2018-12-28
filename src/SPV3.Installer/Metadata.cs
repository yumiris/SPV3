using System.Collections.Generic;
using SPV3.Domain;

namespace SPV3.Installer
{
    /// <summary>
    ///     Type representing the metadata for an SPV3 installation.
    /// </summary>
    public class Metadata
    {
        /// <summary>
        ///     Metadata file version.
        ///     <see cref="Version" />
        /// </summary>
        public Version Version { get; set; }

        /// <summary>
        ///     List of installation packages.
        ///     <see cref="Package" />
        /// </summary>
        public List<Package> Packages { get; set; }

        /// <summary>
        ///     Represent object as Package list.
        /// </summary>
        /// <param name="metadata">
        ///     Object to represent as Package list.
        /// </param>
        /// <returns>
        ///     Package list representation of the object.
        /// </returns>
        public static implicit operator List<Package>(Metadata metadata)
        {
            return metadata.Packages;
        }

        /// <summary>
        ///     Represent Package list as object.
        /// </summary>
        /// <param name="value">
        ///     Package list to represent as object.
        /// </param>
        /// <returns>
        ///     Object representation of the Package list.
        /// </returns>
        public static explicit operator Metadata(List<Package> value)
        {
            return new Metadata
            {
                Packages = value
            };
        }
    }
}