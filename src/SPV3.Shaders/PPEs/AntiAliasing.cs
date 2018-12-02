using SPV3.Shaders.Options;

namespace SPV3.Shaders.PPEs
{
    /// <summary>
    ///     Technique used to smooth otherwise jagged lines or textures by blending the color of an edge with the
    ///     color of pixels around it. The result should be a more pleasing and realistic appearance, depending on
    ///     the intensity of the effect.
    /// </summary>
    public class AntiAliasing
    {
        public const int StateOff = 0x1000;
        public const int StateOn = 0x2000;

        /// <summary>
        ///     <see cref="Toggle" />
        /// </summary>
        public Toggle Toggle { get; set; }
    }
}