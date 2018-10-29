using System.Collections.Generic;
using Atarashii.CLI.Common;
using Atarashii.CLI.Outputs;

namespace Atarashii.CLI
{
    internal class Program : Command
    {
        private const string LoaderCommand = "loader";
        private const string OpenSauceCommand = "opensauce";
        private const string ProfileCommand = "profile";

        private static Dictionary<string, int> Available { get; } = new Dictionary<string, int>
        {
            {LoaderCommand, 0},
            {OpenSauceCommand, 0},
            {ProfileCommand, 0}
        };

        /// <summary>
        ///     Main entry to the Atarashii CLI.
        /// </summary>
        /// <param name="commands">
        ///     The command to invoke. Available commands:
        ///     - loader [load/detect]
        ///     - opensauce [install]
        ///     - profile [resolve]
        /// </param>
        public static void Main(string[] commands)
        {
            Banner.Show();
            Exit.IfIncorrectCommands(commands, Available);

            var command = commands[0].ToLower();
            var args = RemoveComFromArgs(commands);

            switch (command)
            {
                case LoaderCommand:
                    Commands.Loader.Initiate(args);
                    break;
                case OpenSauceCommand:
                    Commands.OpenSauce.Initiate(args);
                    break;
                case ProfileCommand:
                    Commands.Profile.Initiate(args);
                    break;
                default:
                    Exit.WithError("Invalid command provided.", 1);
                    break;
            }
        }
    }
}