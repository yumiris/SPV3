using System;
using System.Diagnostics;
using System.Reflection;

namespace Atarashii.CLI
{
    public static class Banner
    {
        /// <summary>
        ///     File information of the calling assembly.
        /// </summary>
        private static readonly FileVersionInfo Info =
            FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);

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
        public static void ShowBanner()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(Art);
        }
    }
}