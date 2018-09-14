using System;
using System.Text;
using static System.AppDomain;
using static System.Console;
using static System.ConsoleColor;

namespace Miris.Bbkpify.CLI
{
    internal static partial class Program
    {
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
                           // Yu:Miris
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