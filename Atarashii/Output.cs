namespace Atarashii
{
    /// <summary>
    ///     Abstract representing an object that outputs inbound messages.
    /// </summary>
    public abstract class Output
    {
        /// <summary>
        ///     Various types for a log message.
        /// </summary>
        public enum Type
        {
            /// <summary>
            ///     Represents a successful step.
            /// </summary>
            Success,

            /// <summary>
            ///     Represents an informative message.
            /// </summary>
            Info,

            /// <summary>
            ///     Represents an error message.
            /// </summary>
            Error
        }

        /// <summary>
        ///     Logs an inbound message.
        /// </summary>
        /// <param name="type">
        ///     Type of log. <see cref="Type" />
        /// </param>
        /// <param name="subject">
        ///     Subject of the log.
        ///     <example>
        ///         Calling assembly.
        ///     </example>
        /// </param>
        /// <param name="message">
        ///     Actual contents of the log.
        ///     <example>
        ///         Successfully invoked the loading mechanism.
        ///     </example>
        /// </param>
        public abstract void Write(Type type, string subject, string message);
    }
}