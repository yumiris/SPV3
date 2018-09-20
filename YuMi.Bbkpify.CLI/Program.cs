using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using YuMi.Output;

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
        private static readonly List<string> Patterns = new List<string>
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
                Line.Write("Not enough arguments provided. Falling back to manual input.", ConsoleColor.Yellow, "WARN");

                while (!File.Exists(placeholderPath))
                {
                    Line.Write("Provide a valid placeholder file under the size of 8MiB:", ConsoleColor.Cyan, "STEP");
                    placeholderPath = Console.ReadLine();

                    if (placeholderPath != null && File.Exists(placeholderPath))
                    {
                        var fileSize = new FileInfo(placeholderPath).Length; 
                        
                        if (fileSize > Bbkpify.Main.SafeFileSize)
                        {
                            Line.Write($"Placeholder size ({fileSize}) is larger than 8MiB!", ConsoleColor.Red, "STOP");
                            placeholderPath = string.Empty;
                        }
                    }
                }

                while (!Directory.Exists(filesFolderPath))
                {
                    Line.Write("Please provide a valid target directory path:", ConsoleColor.Cyan, "STEP");
                    filesFolderPath = Console.ReadLine();
                }

                while (!Patterns.Contains(fileNamePattern))
                {
                    Line.Write("Please provide a valid file search pattern:", ConsoleColor.Cyan, "STEP");
                    fileNamePattern = Console.ReadLine();
                }
            }
            else
            {
                placeholderPath = args[0];
                filesFolderPath = args[1];
                fileNamePattern = args[2];

                var fileExists = File.Exists(placeholderPath);
                var sizeIsUnder16MiB = new FileInfo(placeholderPath).Length <= Bbkpify.Main.SafeFileSize;
                var directoryExists = Directory.Exists(filesFolderPath);
                var patternIsValid = Patterns.Contains(fileNamePattern);

                // prematurely exit if the following conditions aren't satisfied
                ExitIfFalse(fileExists, "File does not exist.", ExitCodes.InvalidPlaceholderPath);
                ExitIfFalse(sizeIsUnder16MiB, "File is larger than 8MiB.", ExitCodes.PlaceholderFileTooLong);
                ExitIfFalse(directoryExists, "Folder does not exist.", ExitCodes.InvalidFilesFolderPath);
                ExitIfFalse(patternIsValid, "Pattern is invalid.", ExitCodes.InvalidFileNamePattern);
            }

            // if everything is successful, get all files and back them up
            var files = Directory
                .GetFiles(filesFolderPath, $"*{fileNamePattern}*", SearchOption.AllDirectories)
                .Where(x => !x.Contains("multiplayer"))
                .ToArray();

            Bbkpify.Main.ApplyPlaceholderAsync(files, placeholderPath).GetAwaiter().GetResult();
            Line.Write($"\nFinished applying '{placeholderPath}' to '{filesFolderPath}'!", ConsoleColor.Green);
            Environment.Exit((int) ExitCodes.Success);
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
            Line.Write(exitMessage, ConsoleColor.Red, "HALT");
            Console.Error.WriteLine(exitMessage);
            Environment.Exit((int) exitCode);
        }
        
        /// <summary>
        ///     Outputs the main ASCII banner.
        /// </summary>
        private static void ShowBanner()
        {
            Line.Write(Ascii.Banner, ConsoleColor.Magenta);

            // outputs a string with available patterns ...
            // ... and neatly separates each pattern
            // e.g. 'nrml' | 'multi'
            var availablePatterns = new Func<string>(() =>
            {
                var x = new StringBuilder();

                for (var i = 0; i < Patterns.Count; i++)
                {
                    var s = i + 1 == Patterns.Count ? string.Empty : " | ";
                    x.Append($"'{Patterns[i]}'{s}");
                }

                return x.ToString();
            })();

            Line.Write($@"
Usage: .\{AppDomain.CurrentDomain.FriendlyName} <1> <2> <3>
         1 - Placeholder file path (e.g. '.\placeholder.bmp', 'C:\placeholder.bmp')
         2 - Files directory path (e.g. '.\cmt\tags', 'C:\cmt\tags')
         3 - One of the following: {availablePatterns}
", ConsoleColor.Cyan);
        }
    }
}