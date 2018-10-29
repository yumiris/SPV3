using Atarashii.CLI.Outputs;

namespace Atarashii.CLI.Common
{
    /// <summary>
    ///     Abstract type representing an invokable Atarashii CLI command.
    /// </summary>
    public abstract class Command
    {
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