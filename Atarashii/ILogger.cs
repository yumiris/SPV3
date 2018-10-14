namespace Atarashii
{
    /// <summary>
    ///     Interface implemented by logging facilities.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        ///     Adds a given message to the logger.
        /// </summary>
        /// <param name="message">
        ///     Message to append to the log.
        /// </param>
        void Log(string message);
    }
}