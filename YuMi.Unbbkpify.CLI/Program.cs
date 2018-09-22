using System;
using System.IO;
using YuMi.Bbkpify;
using YuMi.Output;

namespace YuMi.Unbbkpify.CLI
{
    /// <summary>
    ///     Main program class.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        ///     Console program entry.
        /// </summary>
        /// <param name="args">
        ///     args[0]: BBKP bitmaps directory path
        /// </param>
        internal static void Main(string[] args)
        {
            ShowBanner();

            var directoryPath = string.Empty;

            if (args.Length == 0)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                while (DirectoryValidator.GetStatus(directoryPath) != DirectoryStatus.IsValid)
                {
                    Line.Write("Please input a valid directory path:", ConsoleColor.Red);
                    directoryPath = Console.ReadLine();
                }
            }
            else
            {
                directoryPath = args[0];

                if (DirectoryValidator.GetStatus(directoryPath) == DirectoryStatus.DoesNotExist)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Error.WriteLine("Target folder does not exist.");
                    Environment.Exit((int)ExitCodes.InvalidFilesFolderPath);
                }
            }

            var files = Directory.GetFiles(directoryPath, $"*.{Bbkpify.Main.Extension}*", SearchOption.AllDirectories);

            try
            {
                Bbkpify.Main.ResetBitmapFiles(files);
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.WriteLine(e.Message);
                Environment.Exit((int)ExitCodes.ExceptionHasBeenThrown);
            }

            Line.Write($"\nFinished restoring bitmaps in '{directoryPath}'!", ConsoleColor.Green);
            Environment.Exit((int)ExitCodes.Success);
        }

        /// <summary>
        ///     Outputs the main ASCII banner.
        /// </summary>
        private static void ShowBanner()
        {
            Line.Write(Ascii.Banner, ConsoleColor.Magenta);
            Line.Write($@"
Usage: .\{AppDomain.CurrentDomain.FriendlyName} <1>
         1 - Directory name to undo the bbkpify process in (e.g. '.\cmt\tags', 'C:\cmt\tags')
", ConsoleColor.Cyan);
        }
    }
}
