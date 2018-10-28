using System.Linq;
using Atarashii.CLI.Common;
using Atarashii.CLI.Outputs;

namespace Atarashii.CLI
{
    internal class Program : Command
    {
        /// <summary>
        ///     Main entry to the Atarashii CLI.
        /// </summary>
        /// <param name="coms">
        ///     The command to invoke. Available commands:
        ///     - loader
        ///     - opensauce
        ///     - profile
        /// </param>
        public static void Main(string[] coms)
        {
            Banner.Show();
            Exit.IfNoArgs(coms);

            var command = coms[0].ToLower();

            if (!Available.Contains(command)) Exit.WithError("Invalid command provided.", 1);

            var args = coms.Skip(1).ToArray();

            switch (command)
            {
                case "loader":
                    Commands.Loader.Initiate(args);
                    break;
                case "opensauce":
                    Commands.OpenSauce.Initiate(args);
                    break;
                case "profile":
                    Commands.Profile.Initiate(args);
                    break;
                default:
                    Commands.Loader.Initiate(args);
                    break;
            }
        }
    }
}