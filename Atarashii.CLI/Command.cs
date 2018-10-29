using System;
using Atarashii.CLI.Outputs;

namespace Atarashii.CLI
{
    /// <summary>
    ///     Abstract type representing an invokable Atarashii CLI command.
    /// </summary>
    public abstract class Command
    {
        /// <summary>
        ///     Initialises the command logic.
        /// </summary>
        /// <param name="commands">
        ///     Additional sub-commands or arguments for the invoked command.
        /// </param>
        public abstract void Initialise(string[] commands);

        /// <summary>
        ///     Informs the user which commands they've invoked.
        /// </summary>
        protected static void HandleInvokeType(InvokeType type, object mainCommand, string subCommand)
        {
            switch (type)
            {
                case InvokeType.Success:
                    Message.Show($"Invoked command '{mainCommand.GetType().Name}.{subCommand}'.", Message.Type.Info);
                    break;
                case InvokeType.Invalid:
                    Exit.WithError($"Invoked command '{mainCommand.GetType().Name}.{subCommand}' is invalid.", 2);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        /// <summary>
        ///     Types of command invokes.
        /// </summary>
        protected enum InvokeType
        {
            /// <summary>
            ///     User has successfully invoked a valid command.
            /// </summary>
            Success,

            /// <summary>
            ///     User has invoked an invalid command.
            /// </summary>
            Invalid
        }
    }
}