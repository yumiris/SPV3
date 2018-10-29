using System.Linq;
using Atarashii.CLI.Outputs;

namespace Atarashii.CLI.Common
{
    /// <summary>
    ///     Abstract type representing an invokable Atarashii CLI command.
    /// </summary>
    public abstract class Command
    {
        /// <summary>
        ///     Removes the command (first argument) from an arguments array.
        /// </summary>
        /// <param name="args">
        ///     Arguments array to remove the command from.
        /// </param>
        /// <returns>
        ///     Arguments array without the command.
        /// </returns>
        protected static string[] RemoveComFromArgs(string[] args)
        {
            return args.Skip(1).ToArray();
        }

        /// <summary>
        ///     Informs the user which commands they've invoked.
        /// </summary>
        /// <param name="command">
        ///     Command they've invoked.
        /// </param>
        /// <param name="subCommand">
        ///     Sub-command they've invoked.
        /// </param>
        protected static void ShowInvokeMessage(string command, string subCommand)
        {
            Message.Show($"Invoked the '{command}.{subCommand}' command.", Message.Type.Info);
        }
    }
}