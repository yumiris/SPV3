using Atarashii.CLI.Common;
using Atarashii.CLI.Outputs;

namespace Atarashii.CLI
{
    internal class Program : Command
    {
        /// <summary>
        ///     Main entry to the Atarashii CLI.
        /// </summary>
        /// <param name="commands">
        ///     The command to invoke.
        /// </param>
        public static void Main(string[] commands)
        {
            Banner.Show();
            Exit.IfNoArgs(commands);

            var args = RemoveComFromArgs(commands);

            switch (commands[0])
            {
                case nameof(Commands.Loader):
                    Commands.Loader.Initialise(args);
                    break;
                case nameof(Commands.OpenSauce):
                    Commands.OpenSauce.Initialise(args);
                    break;
                case nameof(Commands.Profile):
                    Commands.Profile.Initialise(args);
                    break;
                default:
                    Exit.WithError($"Invalid '{nameof(Program)}' command given.", 1);
                    break;
            }
        }
    }
}