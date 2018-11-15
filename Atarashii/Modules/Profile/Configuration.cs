using Atarashii.Modules.Profile.Options;

namespace Atarashii.Modules.Profile
{
    // TODO: Split the types into a dedicated Profile.Options namespace!

    /// <summary>
    ///     This type is used to represent a HCE profile configuration.
    /// </summary>
    public class Configuration
    {
        public Name Name { get; set; } = new Name();
        public Colour Colour { get; set; } = new Colour();
        public Mouse Mouse { get; set; } = new Mouse();
        public Audio Audio { get; set; } = new Audio();
        public Video Video { get; set; } = new Video();
        public Network Network { get; set; } = new Network();
    }

    /// <summary>
    ///     Profile network settings.
    /// </summary>
    public class Network
    {
        /// <summary>
        ///     Connection type settings.
        /// </summary>
        public Connection Connection = new Connection();

        /// <summary>
        ///     Network ports.
        /// </summary>
        public Port Port { get; set; } = new Port();
    }

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

    public class Audio
    {
        public Volume Volume { get; set; } = new Volume();
        public Quality Quality { get; set; } = new Quality();
        public Quality Variety { get; set; } = new Quality();
    }

    public class Mouse
    {
        public Sensitivity Sensitivity { get; set; } = new Sensitivity();
        public bool InvertVerticalAxis { get; set; } = false;
    }
}