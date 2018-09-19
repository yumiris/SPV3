using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using YuMi.Output;
using static System.Console;
using static System.ConsoleColor;
using static System.Environment;
using static YuMi.Bbkpify.Main;
using static YuMi.Bbkpify.ExitCodes;
using static System.AppDomain;
using static YuMi.Bbkpify.Ascii;
using static YuMi.Output.Line;

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
        private static readonly List<string> Types = new List<string>
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
                Write("Not enough arguments provided. Falling back to manual input.", Yellow, "WARN");

                while (!File.Exists(placeholderPath))
                {
                    Write("Please provide a valid placeholder file under the size of 8MiB:", Cyan, "STEP");
                    placeholderPath = ReadLine();

                    if (placeholderPath != null && File.Exists(placeholderPath))
                    {
                        var fileSize = new FileInfo(placeholderPath).Length; 
                        
                        if (fileSize > SafeFileSize)
                        {
                            Write($"Provided placeholder size ({fileSize}) is larger than 8MiB!", Red, "STOP");
                            placeholderPath = string.Empty;
                        }
                    }
                }

                while (!Directory.Exists(filesFolderPath))
                {
                    Write("Please provide a valid target directory path:", Cyan, "STEP");
                    filesFolderPath = ReadLine();
                }

                while (!Types.Contains(fileNamePattern))
                {
                    Write("Please provide a valid file search pattern:", Cyan, "STEP");
                    fileNamePattern = ReadLine();
                }
            }
            else
            {
                placeholderPath = args[0];
                filesFolderPath = args[1];
                fileNamePattern = args[2];

                // prematurely exit if the following conditions aren't satisfied
                ExitIfFalse(File.Exists(placeholderPath), "Placeholder file does not exist.", InvalidPlaceholderPath);
                var sizeIsUnder16MiB = new FileInfo(placeholderPath).Length <= SafeFileSize; 
                ExitIfFalse(sizeIsUnder16MiB, "Placeholder file is larger than 8MiB.", PlaceholderFileTooLong);
                ExitIfFalse(Directory.Exists(filesFolderPath), "Target folder does not exist.", InvalidFilesFolderPath);
                ExitIfFalse(Types.Contains(fileNamePattern), "File name pattern is invalid.", InvalidFileNamePattern);
            }

            // if everything is successful, get all files and back them up
            var files = Directory
                .GetFiles(filesFolderPath, $"*{fileNamePattern}*", SearchOption.AllDirectories)
                .Where(x => !x.Contains("multiplayer"))
                .ToArray();
            
            ApplyPlaceholderAsync(files, placeholderPath).GetAwaiter().GetResult();
            Write($"\nFinished applying '{placeholderPath}' to '{filesFolderPath}'!", Green);
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
            Write(exitMessage, Red, "HALT");
            Error.WriteLine(exitMessage);
            Exit((int) exitCode);
        }
        
        /// <summary>
        ///     Outputs the main ASCII banner.
        /// </summary>
        private static void ShowBanner()
        {
            Write(Banner, Magenta);

            // outputs a string with available patterns ...
            // ... and neatly separates each pattern
            // e.g. 'nrml' | 'multi'
            var availablePatterns = new Func<string>(() =>
            {
                var x = new StringBuilder();

                for (var i = 0; i < Types.Count; i++)
                {
                    var s = i + 1 == Types.Count ? string.Empty : " | ";
                    x.Append($"'{Types[i]}'{s}");
                }

                return x.ToString();
            })();

            Write($@"
Usage: .\{CurrentDomain.FriendlyName} <1> <2> <3>
         1 - Placeholder file path (e.g. '.\placeholder.bmp', 'C:\placeholder.bmp')
         2 - Files directory path (e.g. '.\cmt\tags', 'C:\cmt\tags')
         3 - One of the following: {availablePatterns}
", Cyan);
        }
    }
}