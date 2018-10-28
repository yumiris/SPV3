using System;
using Atarashii.CLI.Common;

namespace Atarashii.CLI.Outputs
{
    public class Banner : Output
    {
        /// <summary>
        ///     CLI-friendly ASCII art banner.
        /// </summary>
        private static string Art => @"
         _                      _     _ _ 
    __ _| |_ __ _ _ __ __ _ ___| |__ (_|_)
   / _` | __/ _` | '__/ _` / __| '_ \| | |
  | (_| | || (_| | | | (_| \__ \ | | | | |
   \__,_|\__\__,_|_|  \__,_|___/_| |_|_|_|
";

        /// <summary>
        ///     Outputs the banner to the CLI.
        /// </summary>
        public static void Show()
        {
            ShowAsciiBanner();
            ShowProductName();
            ShowCompanyName();
            ShowGitRevision();
        }

        /// <summary>
        ///     Outputs the ASCII art of the Atarashii CLI.
        /// </summary>
        private static void ShowAsciiBanner()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(Art);
        }

        /// <summary>
        ///     Outputs the Product Name defined in the Assembly information.
        /// </summary>
        private static void ShowProductName()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("  Program    : ");

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(Exe.ProductName);
        }

        /// <summary>
        ///     Outputs the Company Name defined in the Assembly information.
        /// </summary>
        private static void ShowCompanyName()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("  Developers : ");

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(Exe.CompanyName);
        }

        /// <summary>
        ///     Outputs the Git revision that was used to build the binary.
        /// </summary>
        private static void ShowGitRevision()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("  Revision   : ");

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(Git.Revision);
        }
    }
}