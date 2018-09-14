using System;
using System.IO;
using System.Linq;

namespace Miris.Bbkpify.CLI
{
    internal static class Program
    {
        private const string Extension = "bbkp";

        private static readonly string[] Types = {
            "nrml",
            "multi"
        };

        public static void Main(string[] args)
        {
            ExitIfFalse(args.Length >= 3, "Not enough arguments provided.", 1);

            var placeholderPath = args[0];
            var filesFolderPath = args[1];
            var fileNamePattern = args[2];

            ExitIfFalse(File.Exists(placeholderPath), "Provided placeholder file does not exist.", 2);
            ExitIfFalse(Directory.Exists(filesFolderPath), "Provided files directory does not exist.", 3);
            ExitIfFalse(Types.Contains(fileNamePattern), "Provided file name pattern is invalid.", 4);

            var files = Directory.GetFiles(filesFolderPath, $"*{fileNamePattern}*");

            foreach (var file in files)
            {
                var bbkpFile = $"{file}.{Extension}";
                
                if (!File.Exists(bbkpFile))
                {
                    Console.WriteLine($"Handling ${file}");
                    File.Move(file, bbkpFile);
                    File.Copy(placeholderPath, file);
                }
                else
                {
                    Console.WriteLine($"Skipping ${file}");
                }
            }
        }

        private static void ExitIfFalse(bool condition, string exitMessage, int exitCode)
        {
            if (condition) return;
            Console.WriteLine(exitMessage);
            Environment.Exit(exitCode);
        }
    }
}