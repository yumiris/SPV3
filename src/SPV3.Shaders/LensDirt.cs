namespace SPV3.Shaders
{
    /// <summary>
    ///     Effect that shows subtle dirt/scratch effects on the screen.
    /// </summary>
    public class LensDirt
    {
        public const int StateOff = 0x100;
        public const int StateOn = 0x200;

        /// <summary>
        ///     <see cref="Toggle" />
        /// </summary>
        public Toggle Toggle { get; set; }
    }
}