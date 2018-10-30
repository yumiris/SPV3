using System;
using System.IO;

namespace Atarashii.CLI
{
    public class Banner
    {
        /// <summary>
        ///     Outputs the banner to the CLI.
        /// </summary>
        public static void Show()
        {
            ShowAsciiBanner();
            ShowProductName();
            ShowCompanyName();
            ShowGitRevision();
            ShowProgramHelp();
        }

        /// <summary>
        ///     Outputs the ASCII art of the Atarashii CLI.
        /// </summary>
        private static void ShowAsciiBanner()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;

            using (var reader = new StringReader(ResourceFactory.Get(ResourceFactory.Type.Banner).Text))
            {
                string line;
                while ((line = reader.ReadLine()) != null) Console.WriteLine("  " + line);
            }

            Console.WriteLine(string.Empty);
        }

        /// <summary>
        ///     Outputs the Product Name defined in the Assembly information.
        /// </summary>
        private static void ShowProductName()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("  Program    : ");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("  " + Assembly.ProductName);
        }

        /// <summary>
        ///     Outputs the Company Name defined in the Assembly information.
        /// </summary>
        private static void ShowCompanyName()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("  Developers : ");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("  " + Assembly.CompanyName);
        }

        /// <summary>
        ///     Outputs the Git revision that was used to build the binary.
        /// </summary>
        private static void ShowGitRevision()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("  Revision   : ");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("  " + Git.Revision);
        }

        /// <summary>
        ///     Shows CLI help text.
        /// </summary>
        private static void ShowProgramHelp()
        {
            Console.ForegroundColor = ConsoleColor.Gray;

            using (var reader = new StringReader(ResourceFactory.Get(ResourceFactory.Type.Usage).Text))
            {
                string line;
                while ((line = reader.ReadLine()) != null) Console.WriteLine("  " + line);
            }

            Console.WriteLine(string.Empty);
        }
    }
}