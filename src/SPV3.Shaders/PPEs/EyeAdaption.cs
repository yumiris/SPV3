using SPV3.Shaders.Options;

namespace SPV3.Shaders.PPEs
{
    /// <summary>
    ///     This effect automatically adjust the scene exposure based on ambient light. Unlike commonly used
    ///     implementations of this effect, the adaptation is instant.
    /// </summary>
    public class EyeAdaption
    {
        public const int StateOff = 0x400;
        public const int StateOn = 0x800;

        /// <summary>
        ///     <see cref="Toggle" />
        /// </summary>
        public Toggle Toggle { get; set; }
    }
}