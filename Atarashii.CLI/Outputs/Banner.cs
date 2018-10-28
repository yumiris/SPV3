using System;
using Atarashii.CLI.Common;

namespace Atarashii.CLI.Outputs
{
    public class Banner : Output
    {
        /// <summary>
        ///     CLI-friendly ASCII art banner.
        /// </summary>
        private static string Art => $@"
         _                      _     _ _ 
    __ _| |_ __ _ _ __ __ _ ___| |__ (_|_)
   / _` | __/ _` | '__/ _` / __| '_ \| | |
  | (_| | || (_| | | | (_| \__ \ | | | | |
   \__,_|\__\__,_|_|  \__,_|___/_| |_|_|_|

  Program    : {Info.ProductName}
  Developers : {Info.CompanyName}
";

        /// <summary>
        ///     Outputs the banner to the CLI.
        /// </summary>
        public static void Show()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(Art);
        }
    }
}