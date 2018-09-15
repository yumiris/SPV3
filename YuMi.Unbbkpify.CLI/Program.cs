using System;
using System.IO;
using YuMi.Bbkpify;
using static System.Console;
using static System.ConsoleColor;
using static System.Environment;
using static YuMi.Bbkpify.Main;
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
            
            try
            {
                var files = Directory.GetFiles(directoryPath, $"*.{Extension}*");
                ResetBitmapFiles(files);
            }
            catch (Exception e)
            {
                ForegroundColor = Red;
                WriteLine(e.Message);
                Exit((int)ExceptionHasBeenThrown);
            }
            
            WriteLine($"\nFinished restoring bitmaps in '{directoryPath}'!");
            Exit((int)Success);
        }
    }
}