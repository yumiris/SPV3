using System;
using System.IO;

namespace Miris.Bbkpify.CLI
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length < 3)
            {
                Environment.Exit(1);
            }

            var placeholderPath = args[0];
            var filesFolderPath = args[1];
            var fileNamePattern = args[2];

            if (!File.Exists(placeholderPath))
            {
                Environment.Exit(2);
            }

            if (!Directory.Exists(filesFolderPath))
            {
                Environment.Exit(3);
            }

            if (!fileNamePattern.Equals("nrml") && !fileNamePattern.Equals("multi"))
            {
                Environment.Exit(4);
            }

            var files = Directory.GetFiles(filesFolderPath, $"*{fileNamePattern}*");

            foreach (var file in files)
            {
                if (!file.Contains(".bbkp"))
                {
                    File.Move(file, $"{file}.bbkp");
                    File.Copy(placeholderPath, file);
                }
            }
        }
    }
}