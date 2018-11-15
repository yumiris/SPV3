namespace Atarashii.Modules.Profile
{
    // TODO: Create dedicated types for values such as Colour, Texture & Sound Quality, etc.!
    // TODO: Split the types into a dedicated Profile.Options namespace!

    /// <summary>
    ///     This type is used to represent a HCE profile configuration.
    /// </summary>
    public class Configuration
    {
        public string Name { get; set; } = "New001";
        public Types.Colour Colour { get; set; } = Types.Colour.White;
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
        ///     Settings for the networking system.
        /// </summary>
        public Types.Connection Connection { get; set; } = Types.Connection.DslAverage;

        /// <summary>
        ///     Network port the server listens on.
        /// </summary>
        public ushort ServerPort { get; set; } = 2302;

        /// <summary>
        ///     Network port the client sends on.
        /// </summary>
        public ushort ClientPort { get; set; } = 2303;
    }

    /// <summary>
    ///     Profile video settings.
    /// </summary>
    public class Video
    {
        public ushort Width { get; set; } = 800;
        public ushort Height { get; set; } = 600;
        public uint Refreshrate { get; set; } = 60;
        public Types.Framerate Framerate { get; set; } = Types.Framerate.Fps30;
        public bool Specular { get; set; } = true;
        public bool Shadows { get; set; } = true;
        public bool Decals { get; set; } = true;
        public Levels.Particles Particles { get; set; } = Levels.Particles.High;
        public Levels.Quality Quality { get; set; } = Levels.Quality.High;
    }

    public class Audio
    {
        public uint MasterVolume { get; set; } = 10;
        public uint EffectsVolume { get; set; } = 10;
        public uint MusicVolume { get; set; } = 10;
        public Levels.Quality Quality { get; set; } = Levels.Quality.High;
        public Levels.Quality Variety { get; set; } = Levels.Quality.High;
    }

    public class Mouse
    {
        public uint HorizontalSensitivity { get; set; } = 3;
        public uint VerticalSensitivity { get; set; } = 3;
        public bool InvertVerticalAxis { get; set; } = false;
    }
}