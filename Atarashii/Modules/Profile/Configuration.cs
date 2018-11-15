using Atarashii.Modules.Profile.Options;

namespace Atarashii.Modules.Profile
{
    // TODO: Controls/gamepad settings.
    
    /// <summary>
    ///     This type is used to represent a HCE profile configuration.
    /// </summary>
    public class Configuration
    {
        /// <summary>
        ///     Player name.
        /// </summary>
        public Name Name { get; set; } = new Name();
        
        /// <summary>
        ///     Player colour.
        /// </summary>
        public Colour Colour { get; set; } = new Colour();
        
        /// <summary>
        ///     Mouse settings.
        /// </summary>
        public Mouse Mouse { get; set; } = new Mouse();
        
        /// <summary>
        ///     Audio settings.
        /// </summary>
        public Audio Audio { get; set; } = new Audio();
        
        /// <summary>
        ///     Video settings.
        /// </summary>
        public Video Video { get; set; } = new Video();
        
        /// <summary>
        ///     Network settings.
        /// </summary>
        public Network Network { get; set; } = new Network();
    }
}