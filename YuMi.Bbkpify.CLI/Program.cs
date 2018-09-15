using System;
using System.IO;
using System.Linq;
using System.Text;
using static System.Console;
using static System.ConsoleColor;
using static System.Environment;
using static YuMi.Bbkpify.Main;
using static YuMi.Bbkpify.ExitCodes;
using static System.AppDomain;
using static YuMi.Bbkpify.Ascii;

namespace YuMi.Bbkpify.CLI
{
    /// <summary>
    ///     Main program class.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        ///     Allowed file search patterns.
        /// </summary>
        private static readonly string[] Types =
        {
            "nrml",
            "multi"
        };

        /// <summary>
        ///     Console program entry.
        /// </summary>
        /// <param name="args">
        ///     args[0]: Placeholder path
        ///     args[1]: Target directory path
        ///     args[2]: File search pattern
        /// </param>
        public static void Main(string[] args)
        {
            ShowBanner();

            var placeholderPath = string.Empty;
            var filesFolderPath = string.Empty;
            var fileNamePattern = string.Empty;

            if (args.Length < 3)
            {
                ForegroundColor = Red;
                WriteLine("Not enough arguments provided. Falling back to manual input.");

                ForegroundColor = Cyan;
                while (!File.Exists(placeholderPath))
                {
                    WriteLine("Please provide a valid placeholder file path:");
                    placeholderPath = ReadLine();
                    ForegroundColor = Red;
                }

                ForegroundColor = Cyan;
                while (!Directory.Exists(filesFolderPath))
                {
                    WriteLine("Please provide a valid target directory path:");
                    filesFolderPath = ReadLine();
                    ForegroundColor = Red;
                }

                ForegroundColor = Cyan;
                while (!Types.Contains(fileNamePattern))
                {
                    WriteLine("Please provide a valid file search pattern:");
                    fileNamePattern = ReadLine();
                    ForegroundColor = Red;
                }
            }
            else
            {
                placeholderPath = args[0];
                filesFolderPath = args[1];
                fileNamePattern = args[2];

                // prematurely exit if the following conditions aren't satisfied
                ExitIfFalse(File.Exists(placeholderPath), "Placeholder file does not exist.", InvalidPlaceholderPath);
                ExitIfFalse(Directory.Exists(filesFolderPath), "Target folder does not exist.", InvalidFilesFolderPath);
                ExitIfFalse(Types.Contains(fileNamePattern), "File name pattern is invalid.", InvalidFileNamePattern);
            }

            // if everything is successful, get all files and back them up
            var files = Directory.GetFiles(filesFolderPath, $"*{fileNamePattern}*");
            ApplyPlaceholder(files, placeholderPath);

            ForegroundColor = Green;
            WriteLine($"\nFinished applying '{placeholderPath}' to '{filesFolderPath}'!");
            Exit((int) Success);
        }

        /// <summary>
        ///     Exit the application if the inbound condition is false.
        /// </summary>
        /// <param name="condition">Condition outcome to check.</param>
        /// <param name="exitMessage">Message to write to the console upon exit.</param>
        /// <param name="exitCode">Application exit code to use upon exit.</param>
        private static void ExitIfFalse(bool condition, string exitMessage, ExitCodes exitCode)
        {
            if (condition) return;
            ForegroundColor = Red;
            WriteLine(exitMessage);
            Exit((int) exitCode);
        }
        
        /// <summary>
        ///     Outputs the main ASCII banner.
        /// </summary>
        private static void ShowBanner()
        {
            ForegroundColor = Magenta;
            WriteLine(Banner);

            // outputs a string with available patterns ...
            // ... and neatly separates each pattern
            // e.g. 'nrml' | 'multi'
            var availablePatterns = new Func<string>(() =>
            {
                var x = new StringBuilder();

                for (var i = 0; i < Types.Length; i++)
                {
                    var s = i + 1 == Types.Length ? string.Empty : " | ";
                    x.Append($"'{Types[i]}'{s}");
                }

                return x.ToString();
            })();

            ForegroundColor = Cyan;
            WriteLine($@"
Usage: .\{CurrentDomain.FriendlyName} <1> <2> <3>
         1 - Placeholder file path (e.g. '.\placeholder.bmp', 'C:\placeholder.bmp')
         2 - Files directory path (e.g. '.\cmt\tags', 'C:\cmt\tags')
         3 - One of the following: {availablePatterns}
");
        }
    }
}