using SPV3.Shaders.Options;

namespace SPV3.Shaders.PPEs
{
    /// <summary>
    ///     Effect which eliminated banding artifacts (sort of band like anomalies that appear in otherwise smooth
    ///     gradients of colors, due to small color space/bit depth).
    /// </summary>
    public class Debanding
    {
        public const int StateOff = 0x1000;
        public const int StateLow = 0x2000;
        public const int StateHigh = 0x4000;

        /// <summary>
        ///     <see cref="Level" />
        /// </summary>
        public Level Level { get; set; } = Level.Off;
    }
}