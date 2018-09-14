using System;
using System.IO;
using System.Linq;
using static System.Console;
using static System.ConsoleColor;
using static System.Environment;
using static System.IO.File;
using static Miris.Bbkpify.CLI.ExitCodes;

namespace Miris.Bbkpify.CLI
{
    internal enum ExitCodes
    {
        Success = 0,
        MoreArgumentsNecessary,
        InvalidPlaceholderPath,
        InvalidFilesFolderPath,
        InvalidFileNamePattern
    }
    
    internal static partial class Program
    {
        private const string Extension = "bbkp";

        private static readonly string[] Types = {
            "nrml",
            "multi"
        };
        
        public static void Main(string[] args)
        {
            ShowBanner();
            
            ExitIfFalse(args.Length >= 3, "Not enough arguments provided.", MoreArgumentsNecessary);

            var placeholderPath = args[0];
            var filesFolderPath = args[1];
            var fileNamePattern = args[2];

            ExitIfFalse(Exists(placeholderPath), "Provided placeholder file does not exist.", InvalidPlaceholderPath);
            ExitIfFalse(Directory.Exists(filesFolderPath), "Provided files directory does not exist.", InvalidFilesFolderPath);
            ExitIfFalse(Types.Contains(fileNamePattern), "Provided file name pattern is invalid.", InvalidFileNamePattern);

            var files = Directory.GetFiles(filesFolderPath, $"*{fileNamePattern}*");
            BbkpifyFiles(files, placeholderPath);

            ForegroundColor = Green;
            WriteLine($"\nFinished applying '{placeholderPath}' to '{filesFolderPath}'!");
            Exit((int)Success);
        }

        private static void BbkpifyFiles(string[] files, string placeholderPath)
        {
            for (int i = 0; i < files.Length; i++)
            {
                var file = files[i];
                var bbkpFile = $"{file}.{Extension}";
                var progress = new Func<string>(() => $"[{i + 1}/{files.Length}]")();

                if (!file.Contains(Extension) && !Exists(bbkpFile))
                {
                    ForegroundColor = Green;
                    WriteLine($"{progress}\t| HANDLING {file}");
                    Move(file, bbkpFile);
                    Copy(placeholderPath, file);
                }
                else
                {
                    ForegroundColor = Yellow;
                    WriteLine($"{progress}\t| SKIPPING {file}");
                }
            }
        }

        private static void ExitIfFalse(bool condition, string exitMessage, ExitCodes exitCode)
        {
            if (condition) return;
            ForegroundColor = Red;
            WriteLine(exitMessage);
            Exit((int)exitCode);
        }
    }
}