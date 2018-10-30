using Atarashii.Loader;

namespace Atarashii.CLI
{
    /// <summary>
    ///     Abstract type representing an invokable Atarashii CLI command.
    /// </summary>
    public abstract class Command
    {
        private readonly Output _output;

        protected Command(Output output)
        {
            _output = output;
        }

        /// <summary>
        ///     Initialises the command logic.
        /// </summary>
        /// <param name="commands">
        ///     Additional sub-commands or arguments for the invoked command.
        /// </param>
        public abstract void Initialise(string[] commands);

        /// <summary>
        ///     Outputs a Type.Success message.
        /// </summary>
        /// <param name="message">
        ///     Message to write.
        /// </param>
        protected void Pass(string message)
        {
            _output?.Write(Output.Type.Success, Executable.Name, message);
        }

        /// <summary>
        ///     Outputs a Type.Info message.
        /// </summary>
        /// <param name="message">
        ///     Message to write.
        /// </param>
        protected void Info(string message)
        {
            _output?.Write(Output.Type.Info, Executable.Name, message);
        }

        /// <summary>
        ///     Outputs the inbound error code and exits the application.
        /// </summary>
        /// <param name="message">
        ///     Message to output.
        /// </param>
        /// <param name="code">
        ///     Error code to use for exiting.
        /// </param>
        protected void Fail(string message, int code)
        {
            _output?.Write(Output.Type.Error, Executable.Name, message);
            Exit.WithError(message, code);
        }
    }
}