using System;

namespace Atarashii.CLI.Outputs
{
    public class Banner : Output
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
            WriteLineMulti(new Resource("Atarashii.CLI.BANNER").Text);
        }

        /// <summary>
        ///     Outputs the Product Name defined in the Assembly information.
        /// </summary>
        private static void ShowProductName()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Write("Program    : ");

            Console.ForegroundColor = ConsoleColor.Cyan;
            WriteLine(Assembly.ProductName);
        }

        /// <summary>
        ///     Outputs the Company Name defined in the Assembly information.
        /// </summary>
        private static void ShowCompanyName()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Write("Developers : ");

            Console.ForegroundColor = ConsoleColor.Cyan;
            WriteLine(Assembly.CompanyName);
        }

        /// <summary>
        ///     Outputs the Git revision that was used to build the binary.
        /// </summary>
        private static void ShowGitRevision()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Write("Revision   : ");

            Console.ForegroundColor = ConsoleColor.Cyan;
            WriteLine(Git.Revision);
        }

        /// <summary>
        ///     Shows CLI help text.
        /// </summary>
        private static void ShowProgramHelp()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            WriteLineMulti(new Resource("Atarashii.CLI.USAGE.md").Text);
        }
    }
}