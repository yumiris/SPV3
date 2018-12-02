using SPV3.Shaders.Options;

namespace SPV3.Shaders.PPEs
{
    /// <summary>
    ///     Effect which eliminated banding artifacts (sort of band like anomalies that appear in otherwise smooth
    ///     gradients of colors, due to small color space/bit depth).
    /// </summary>
    public class Debanding
    {
        public const int StateOff = 0x4000;
        public const int StateLow = 0x8000;
        public const int StateHigh = 0x10000;

        /// <summary>
        ///     <see cref="Level" />
        /// </summary>
        public Level Level { get; set; }
    }
}