using SPV3.Shaders.Options;

namespace SPV3.Shaders.PPEs
{
    /// <summary>
    ///     Effect in which objects within some range of distances in a scene appear in focus, and objects nearer or
    ///     farther than this range appear out of focus or blurred out. Just like any camera or your eye, ADOF
    ///     enables auto-focusing.
    /// </summary>
    public class DepthOfField
    {
        public const int StateOff = 0x8;
        public const int StateLow = 0x10;
        public const int StateHigh = 0x20;

        /// <summary>
        ///     <see cref="Level" />
        /// </summary>
        public Level Level { get; set; } = Level.Off;
    }
}