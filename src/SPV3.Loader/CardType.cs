namespace SPV3.Loader
{
    /// <summary>
    ///     Represents the card (GPU) types that HCE should be forced run as.
    /// </summary>
    public enum CardType
    {
        /// <summary>
        ///     Forces the game to run as a fixed function card.
        /// </summary>
        FixedFunction,

        /// <summary>
        ///     Forces the game to run as a shader 1.1 card.
        /// </summary>
        Shaders11Card,

        /// <summary>
        ///     Forces the game to run as a shader 1.4 card.
        /// </summary>
        Shaders14Card,

        /// <summary>
        ///     Forces the game to run as a shader 2.0 card.
        /// </summary>
        Shaders20Card
    }
}