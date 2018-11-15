namespace Atarashii.Modules.Profile.Options
{
    /// <summary>
    ///     Representation of the network port settings.
    /// </summary>
    public class Port
    {
        /// <summary>
        ///     The port value the client sends data from.
        /// </summary>
        public ushort Client = 2303;

        /// <summary>
        ///     The port value the server listens on.
        /// </summary>
        public ushort Server = 2302;
    }
}