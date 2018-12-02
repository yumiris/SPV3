namespace BalsamV.Settings
{
    /// <summary>
    ///     Connection types.
    /// </summary>
    public enum Connection
    {
        /// <summary>
        ///     56kbps setting - allows a maximum of 2 people to join a hosted server.
        /// </summary>
        Modem,

        /// <summary>
        ///     DSL/Cable (LOW) setting - allows a maximum of 4 people to join a hosted server.
        /// </summary>
        DslLow,

        /// <summary>
        ///     DSL/Cable (AVG) setting - allows a maximum of 8 people to join a hosted server.
        /// </summary>
        DslAverage,

        /// <summary>
        ///     DSL/Cable (HIGH) setting - allows a maximum of 10 people to join a hosted server.
        /// </summary>
        DslHigh,

        /// <summary>
        ///     Represents the 11/LAN setting - allows a maximum of 16 people to join a hosted server.
        /// </summary>
        Lan
    }
}