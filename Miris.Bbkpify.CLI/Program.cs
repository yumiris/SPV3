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
            if (args.Length < 3)
            {
                Console.WriteLine("Not enough arguments provided.");
                Environment.Exit(1);
            }

            var placeholderPath = args[0];
            var filesFolderPath = args[1];
            var fileNamePattern = args[2];

            if (!File.Exists(placeholderPath))
            {
                Console.WriteLine("Provided placeholder file does not exist.");
                Environment.Exit(2);
            }

            if (!Directory.Exists(filesFolderPath))
            {
                Console.WriteLine("Provided files directory does not exist.");
                Environment.Exit(3);
            }

            if (Types.Contains(fileNamePattern) == false)
            {
                Console.WriteLine("Provided file name pattern is invalid.");
                Environment.Exit(4);
            }

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
    }
}