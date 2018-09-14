using System;
using System.IO;
using System.Linq;
using static System.Console;
using static System.ConsoleColor;
using static System.Environment;
using static Miris.Bbkpify.CLI.ExitCodes;

namespace Miris.Bbkpify.CLI
{
    /// <summary>
    /// Main application exit codes.
    /// </summary>
    internal enum ExitCodes
    {
        Success = 0,
        InvalidPlaceholderPath,
        InvalidFilesFolderPath,
        InvalidFileNamePattern
    }

    /// <summary>
    /// Logical portion of the program.
    /// </summary>
    internal static partial class Program
    {
        /// <summary>
        /// File extension to use for the backup file.
        /// </summary>
        private const string Extension = "bbkp";

        /// <summary>
        /// Allowed file search patterns.
        /// </summary>
        private static readonly string[] Types =
        {
            "nrml",
            "multi"
        };

        /// <summary>
        /// Console program entry.
        /// </summary>
        /// <param name="args">
        /// args[0]: Placeholder path
        /// args[1]: Target directory path
        /// args[2]: File search pattern
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
            BbkpifyFiles(files, placeholderPath);

            ForegroundColor = Green;
            WriteLine($"\nFinished applying '{placeholderPath}' to '{filesFolderPath}'!");
            Exit((int) Success);
        }

        private static void BbkpifyFiles(string[] files, string placeholderPath)
        {
            for (int i = 0; i < files.Length; i++)
            {
                var file = files[i];
                var bbkpFile = $"{file}.{Extension}";

                // looks like: [1/10]
                var progress = new Func<string>(() => $"[{i + 1}/{files.Length}]")();

                // check if the current file has been handled in a previous execution
                if (!file.Contains(Extension) && !File.Exists(bbkpFile))
                {
                    ForegroundColor = Green;
                    WriteLine($"{progress}\t| HANDLING {file}");
                    
                    // backup through renaming, and replace with the placeholder
                    File.Move(file, bbkpFile);
                    File.Copy(placeholderPath, file);
                }
                else
                {
                    ForegroundColor = Yellow;
                    WriteLine($"{progress}\t| SKIPPING {file}");
                }
            }
        }

        /// <summary>
        /// Exit the application if the inbound condition is false.
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
    }
}