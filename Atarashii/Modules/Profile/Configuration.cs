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
        ///     Network port the server listens on.
        /// </summary>
        public Port ServerPort { get; set; } = new Port();

        /// <summary>
        ///     Network port the client sends on.
        /// </summary>
        public Port ClientPort { get; set; } = new Port();
    }

    /// <summary>
    ///     Profile video settings.
    /// </summary>
    public class Video
    {
        public Dimension Width { get; set; } = new Dimension();
        public Dimension Height { get; set; } = new Dimension();
        public RefreshRate RefreshRate { get; set; } = new RefreshRate();
        public FrameRate FrameRate { get; set; } = new FrameRate();
        public bool Specular { get; set; } = true;
        public bool Shadows { get; set; } = true;
        public bool Decals { get; set; } = true;
        public Particles Particles { get; set; } = new Particles();
        public Levels Quality { get; set; } = new Levels();
    }

    public class Audio
    {
        public Volume Master { get; set; } = new Volume();
        public Volume Effects { get; set; } = new Volume();
        public Volume Music { get; set; } = new Volume();
        public Levels Quality { get; set; } = new Levels();
        public Levels Variety { get; set; } = new Levels();
    }

    public class Mouse
    {
        public Sensitivity Horizontal { get; set; } = new Sensitivity();
        public Sensitivity Vertical { get; set; } = new Sensitivity();
        public bool InvertVerticalAxis { get; set; } = false;
    }
}