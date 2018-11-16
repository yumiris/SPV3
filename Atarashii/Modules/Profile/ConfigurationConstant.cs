namespace Atarashii.Modules.Profile
{
    /// <summary>
    ///     Blam.sav properties offsets & lengths.
    /// </summary>
    public abstract class ConfigurationConstant
    {
        /// <summary>
        ///     Length of the blam.sav binary.
        /// </summary>
        public const int BlamLength = 0x2000;

        /// <summary>
        ///     Data length of the profile name property.
        /// </summary>
        public const int NameOffset = 0x2;

        /// <summary>
        ///     Offset of the profile name property.
        /// </summary>
        public const int NameLength = 0xB;
    }
}