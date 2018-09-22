using System;
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
        private static bool firstRun = true;

        /// <summary>
        ///     Console program entry.
        /// </summary>
        /// <param name="args">
        ///     args[0]: Placeholder bitmap path
        ///     args[1]: Target bitmaps directory path
        ///     args[2]: Bitmaps search pattern
        /// </param>
        public static void Main(string[] args)
        {
            if (firstRun)
            {
                ShowBanner();
            }

            var bitmapPlaceholder = string.Empty;
            var bitmapsDirectory = string.Empty;
            var bitmapsPattern = string.Empty;

            if (args.Length < 3)
            {
                if (firstRun)
                {
                    Line.Write("Not enough arguments provided. Using manual input...", ConsoleColor.Yellow, "WARN");
                }
                else
                {
                    Line.Write("Press Ctrl-C to exit, or continue with the following...", ConsoleColor.Yellow, "INFO");
                }

                while (PlaceholderValidator.GetStatus(bitmapPlaceholder) != PlaceholderStatus.IsValid)
                {
                    Line.Write("Please type a valid placeholder file under the size of 8MiB:", ConsoleColor.Cyan, "STEP");
                    bitmapPlaceholder = Console.ReadLine();

                    if (bitmapPlaceholder != null && File.Exists(bitmapPlaceholder))
                    {
                        if (PlaceholderValidator.GetStatus(bitmapPlaceholder) == PlaceholderStatus.IsTooLarge)
                        {
                            Line.Write($"Placeholder file is larger than 8MiB!", ConsoleColor.Red, "STOP");
                            bitmapPlaceholder = string.Empty;
                        }
                    }
                }

                while (DirectoryValidator.GetStatus(bitmapsDirectory) != DirectoryStatus.IsValid)
                {
                    Line.Write("Please type a valid target directory path:", ConsoleColor.Cyan, "STEP");
                    bitmapsDirectory = Console.ReadLine();
                }

                while (PatternValidator.GetStatus(bitmapsPattern) != PatternStatus.IsValid)
                {
                    Line.Write("Please type a valid file search pattern:", ConsoleColor.Cyan, "STEP");
                    bitmapsPattern = Console.ReadLine();
                }
            }
            else
            {
                bitmapPlaceholder = args[0];
                bitmapsDirectory = args[1];
                bitmapsPattern = args[2];

                var placeholderStatus = PlaceholderValidator.GetStatus(bitmapPlaceholder);
                var fileExists = placeholderStatus != PlaceholderStatus.DoesNotExist;
                var sizeIsUnder16MiB = placeholderStatus != PlaceholderStatus.IsTooLarge;

                var directoryExists = DirectoryValidator.GetStatus(bitmapsDirectory) != DirectoryStatus.DoesNotExist;
                var patternIsValid = PatternValidator.GetStatus(bitmapsPattern) != PatternStatus.IsInvalid;

                // prematurely exit if the following conditions aren't satisfied
                ExitIfFalse(fileExists, "Placeholder does not exist.", ExitCodes.InvalidPlaceholderPath);
                ExitIfFalse(sizeIsUnder16MiB, "Placeholder is larger than 8MiB.", ExitCodes.PlaceholderFileTooLong);
                ExitIfFalse(directoryExists, "Bitmaps directory does not exist.", ExitCodes.InvalidFilesFolderPath);
                ExitIfFalse(patternIsValid, "Searcg pattern is invalid.", ExitCodes.InvalidFileNamePattern);
            }

            // if everything is successful, get all files and back them up
            var files = Directory
                .GetFiles(bitmapsDirectory, $"*{bitmapsPattern}*.bitmap", SearchOption.AllDirectories)
                .Where(x => !x.Contains("multiplayer"))
                .ToArray();

            Bbkpify.Main.ApplyPlaceholderAsync(files, bitmapPlaceholder).GetAwaiter().GetResult();

            Line.Write($"\nFinished applying '{bitmapPlaceholder}' to '{bitmapsDirectory}'!", ConsoleColor.Green);

            firstRun = false;
            Main(new string[] { });
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
            Environment.Exit((int)exitCode);
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

                for (var i = 0; i < Bbkpify.Main.Patterns.Count; i++)
                {
                    var s = i + 1 == Bbkpify.Main.Patterns.Count ? string.Empty : " | ";
                    x.Append($"'{Bbkpify.Main.Patterns[i]}'{s}");
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
