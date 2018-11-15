using Atarashii.Modules.Profile.Options;

namespace Atarashii.Modules.Profile
{
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
}