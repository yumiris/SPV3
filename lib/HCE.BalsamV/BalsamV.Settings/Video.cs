namespace BalsamV.Settings
{
    /// <summary>
    ///     Representation of the profile video settings.
    /// </summary>
    public class Video
    {
        /// <summary>
        ///     Video resolution settings.
        /// </summary>
        public Resolution Resolution { get; set; } = new Resolution();

        /// <summary>
        ///     Video refresh rate settings.
        /// </summary>
        public RefreshRate RefreshRate { get; set; } = new RefreshRate();

        /// <summary>
        ///     Video frame rate settings.
        /// </summary>
        public FrameRate FrameRate { get; set; } = FrameRate.Fps30;

        /// <summary>
        ///     Video effects settings.
        /// </summary>
        public Effects Effects { get; set; } = new Effects();

        /// <summary>
        ///     Video particles settings.
        /// </summary>
        public Particles Particles { get; set; } = Particles.High;

        /// <summary>
        ///     Video texture quality settings.
        /// </summary>
        public Quality Quality { get; set; } = Quality.High;
    }
}