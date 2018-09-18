using System;
using System.IO;
using static System.Console;
using static System.ConsoleColor;
using static System.Environment;
using static YuMi.Bbkpify.Main;
using static YuMi.Bbkpify.ExitCodes;
using static System.AppDomain;
using static YuMi.Bbkpify.Ascii;
using static YuMi.Output.Line;

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
                ForegroundColor = Cyan;
                while (!Directory.Exists(directoryPath))
                {
                    Write("Please input a valid directory path:", Red);
                    directoryPath = ReadLine();
                }
            }
            else
            {
                directoryPath = args[0];

                if (!Directory.Exists(directoryPath))
                {
                    ForegroundColor = Red;
                    Error.WriteLine("Target folder does not exist.");
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
                Error.WriteLine(e.Message);
                Exit((int)ExceptionHasBeenThrown);
            }
            
            Write($"\nFinished restoring bitmaps in '{directoryPath}'!", Green);
            Exit((int)Success);
        }
        
        /// <summary>
        ///     Outputs the main ASCII banner.
        /// </summary>
        private static void ShowBanner()
        {
            Write(Banner, Magenta);
            Write($@"
Usage: .\{CurrentDomain.FriendlyName} <1>
         1 - Directory name to undo the bbkpify process in (e.g. '.\cmt\tags', 'C:\cmt\tags')
", Cyan);
        }
    }
}