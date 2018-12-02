using SPV3.Shaders.Options;

namespace SPV3.Shaders.PPEs
{
    /// <summary>
    ///     Effect that shows lens flares automatically based on bright spots in the scene.
    /// </summary>
    public class DynamicFlare
    {
        public const int StateOff = 0x40;
        public const int StateOn = 0x80;

        /// <summary>
        ///     <see cref="Toggle" />
        /// </summary>
        public Toggle Toggle { get; set; }
    }
}