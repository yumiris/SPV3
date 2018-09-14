using System;
using System.IO;
using System.Linq;
using static System.Console;
using static System.ConsoleColor;
using static System.Environment;
using static System.IO.File;

namespace Miris.Bbkpify.CLI
{
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
            
            ExitIfFalse(args.Length >= 3, "Not enough arguments provided.", 1);

            var placeholderPath = args[0];
            var filesFolderPath = args[1];
            var fileNamePattern = args[2];

            ExitIfFalse(Exists(placeholderPath), "Provided placeholder file does not exist.", 2);
            ExitIfFalse(Directory.Exists(filesFolderPath), "Provided files directory does not exist.", 3);
            ExitIfFalse(Types.Contains(fileNamePattern), "Provided file name pattern is invalid.", 4);

            var files = Directory.GetFiles(filesFolderPath, $"*{fileNamePattern}*");

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

        private static void ExitIfFalse(bool condition, string exitMessage, int exitCode)
        {
            if (condition) return;
            ForegroundColor = Red;
            WriteLine(exitMessage);
            Exit(exitCode);
        }
    }
}