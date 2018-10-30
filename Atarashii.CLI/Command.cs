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

        protected void OutputMessage(Output.Type type, string message)
        {
            _output?.Write(type, Executable.Name, message);
        }
    }
}