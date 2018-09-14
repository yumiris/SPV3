using System.IO;
using YuMi.Bbkpify;
using static System.Console;
using static System.ConsoleColor;
using static System.Environment;
using static YuMi.Bbkpify.Core;
using static YuMi.Bbkpify.ExitCodes;

namespace YuMi.Unbbkpify.CLI
{
    internal static partial class Program
    {
        internal static void Main(string[] args)
        {
            ShowBanner();

            var directoryPath = string.Empty;

            if (args.Length == 0)
            {
                ForegroundColor = Cyan;
                while (!Directory.Exists(directoryPath))
                {
                    WriteLine("Please input a valid directory path:");
                    directoryPath = ReadLine();
                    ForegroundColor = Red;
                }
            }
            else
            {
                directoryPath = args[0];

                if (!Directory.Exists(directoryPath))
                {
                    ForegroundColor = Red;
                    WriteLine("Target folder does not exist.");
                    Exit((int)InvalidFilesFolderPath);
                }
            }
            
            var files = Directory.GetFiles(directoryPath, $"*.{Extension}*");
            Revert(files);
            
            WriteLine($"\nFinished restoring bitmaps in '{directoryPath}'!");
            Exit((int)Success);
        }

        private static void UnbkkpifyFiles(string[] files)
        {
            ForegroundColor = Green;
            for (var i = 0; i < files.Length; i++)
            {
                var currentFile = files[i];
                var placeholder = currentFile.Substring(0, currentFile.Length - Extension.Length - 1);

                var progress = Ascii.Progress(i, files.Length);
                WriteLine($"{progress}\t| RESTORING {placeholder}");
                
                File.Delete(placeholder);
                File.Move(currentFile, placeholder);
            }
        }
    }
}