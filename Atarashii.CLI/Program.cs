using System.Collections.Generic;
using System.Linq;

namespace Atarashii.CLI
{
    internal class Program
    {
        /// <summary>
        ///     Available invokable commands.
        /// </summary>
        private static readonly List<string> Commands = new List<string>
        {
            "loader",
            "opensauce",
            "profile"
        };

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
            Banner.ShowBanner();
            Exit.IfNoArgs(coms);

            var command = coms[0].ToLower();

            if (!Commands.Contains(command)) Exit.WithError("Invalid command provided.", 1);

            var args = coms.Skip(1).ToArray();

            switch (command)
            {
                case "loader":
                    Loader.Initiate(args);
                    break;
                case "opensauce":
                    OpenSauce.Initiate(args);
                    break;
                case "profile":
                    Profile.Initiate(args);
                    break;
                default:
                    Loader.Initiate(args);
                    break;
            }
        }
    }
}