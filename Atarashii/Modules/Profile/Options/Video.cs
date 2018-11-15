namespace Atarashii.Modules.Profile.Options
{
    /// <summary>
    ///     Profile video settings.
    /// </summary>
    public class Video
    {
        public Resolution Resolution { get; set; } = new Resolution();
        public RefreshRate RefreshRate { get; set; } = new RefreshRate();
        public FrameRate FrameRate { get; set; } = new FrameRate();
        public bool Specular { get; set; } = true;
        public bool Shadows { get; set; } = true;
        public bool Decals { get; set; } = true;
        public Particles Particles { get; set; } = new Particles();
        public Quality Quality { get; set; } = new Quality();
    }
}