using Atarashii.CLI.Common;
using Atarashii.CLI.Outputs;

namespace Atarashii.CLI
{
    internal class Program : Command
    {
        private const string LoaderCommand = "loader";
        private const string OpenSauceCommand = "opensauce";
        private const string ProfileCommand = "profile";

        /// <summary>
        ///     Main entry to the Atarashii CLI.
        /// </summary>
        /// <param name="coms">
        ///     The command to invoke. Available commands:
        ///     - loader [load/detect]
        ///     - opensauce [install]
        ///     - profile [resolve]
        /// </param>
        public static void Main(string[] coms)
        {
            Banner.Show();
            Exit.IfNoArgs(coms);

            var command = coms[0].ToLower();
            var args = RemoveComFromArgs(coms);

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