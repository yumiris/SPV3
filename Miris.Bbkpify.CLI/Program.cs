using System;
using System.IO;
using System.Linq;
using System.Text;
using static System.AppDomain;
using static System.Console;
using static System.ConsoleColor;
using static System.Environment;
using static System.IO.File;

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
            ShowBanner();
            
            ExitIfFalse(args.Length >= 3, "Not enough arguments provided.", 1);

            var placeholderPath = args[0];
            var filesFolderPath = args[1];
            var fileNamePattern = args[2];

            ExitIfFalse(Exists(placeholderPath), "Provided placeholder file does not exist.", 2);
            ExitIfFalse(Directory.Exists(filesFolderPath), "Provided files directory does not exist.", 3);
            ExitIfFalse(Types.Contains(fileNamePattern), "Provided file name pattern is invalid.", 4);

            var files = Directory.GetFiles(filesFolderPath, $"*{fileNamePattern}*");

            foreach (var file in files)
            {
                var bbkpFile = $"{file}.{Extension}";
                
                if (!Exists(bbkpFile))
                {
                    ForegroundColor = Green;
                    WriteLine($"Handling ${file}");
                    Move(file, bbkpFile);
                    Copy(placeholderPath, file);
                }
                else
                {
                    ForegroundColor = Yellow;
                    WriteLine($"Skipping ${file}");
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

        private static void ShowBanner()
        {
            ForegroundColor = Magenta;
            WriteLine(@"
    __    __    __         _ ____     
   / /_  / /_  / /______  (_) __/_  __
  / __ \/ __ \/ //_/ __ \/ / /_/ / / /
 / /_/ / /_/ / ,< / /_/ / / __/ /_/ / 
/_.___/_.___/_/|_/ .___/_/_/  \__, /  
                /_/          /____/   
======================================
");
            var availableTypes = new Func<string>(() =>
            {
                var x = new StringBuilder();

                for (var i = 0; i < Types.Length; i++)
                {
                    var s = i + 1 == Types.Length ? string.Empty : " | ";
                    x.Append($"'{Types[i]}'{s}");
                }

                return x.ToString();
            })();
            
            ForegroundColor = Cyan;
            WriteLine($@"
Usage: .\{CurrentDomain.FriendlyName} <1> <2> <3>
         1 - Placeholder file path (e.g. '.\placeholder.bmp', 'C:\placeholder.bmp')
         2 - Files directory path (e.g. '.\cmt\tags', 'C:\cmt\tags')
         3 - One of the following: {availableTypes}
");
        }
    }
}