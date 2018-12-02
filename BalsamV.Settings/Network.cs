namespace BalsamV.Settings
{
    /// <summary>
    ///     Profile network settings.
    /// </summary>
    public class Network
    {
        /// <summary>
        ///     Connection type settings.
        /// </summary>
        public Connection Connection { get; set; } = Connection.Modem;

        /// <summary>
        ///     Network ports.
        /// </summary>
        public Port Port { get; set; } = new Port();
    }
}