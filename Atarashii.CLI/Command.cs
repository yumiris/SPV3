using System;
using System.Linq;

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
            _output?.Write(Output.Type.Success, Assembly.ProductName, message);
        }

        /// <summary>
        ///     Outputs a Type.Info message.
        /// </summary>
        /// <param name="message">
        ///     Message to write.
        /// </param>
        protected void Info(string message)
        {
            _output?.Write(Output.Type.Info, Assembly.ProductName, message);
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
            _output?.Write(Output.Type.Error, Assembly.ProductName, message);
            Environment.Exit(code);
        }

        /// <summary>
        ///     Exits the program if the inbound arguments are empty.
        /// </summary>
        /// <param name="args">
        ///     Arguments to check the length of.
        /// </param>
        public void ExitIfNone(string[] args)
        {
            if (args.Length == 0) Exit.WithError("Not enough or commands arguments provided.", 1);
        }

        /// <summary>
        ///     Removes the command (first argument) from an arguments array.
        /// </summary>
        /// <param name="command">
        ///     Arguments array to remove the command from.
        /// </param>
        /// <returns>
        ///     Arguments array without the command.
        /// </returns>
        public static string[] GetArguments(string[] command)
        {
            return command.Skip(1).ToArray();
        }
    }
}