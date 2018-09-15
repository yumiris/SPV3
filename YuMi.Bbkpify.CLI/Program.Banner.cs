using System;
using System.Text;
using static System.AppDomain;
using static System.Console;
using static System.ConsoleColor;
using static YuMi.Bbkpify.Ascii;

namespace YuMi.Bbkpify.CLI
{
    /// <summary>
    ///     Visual portion of the program.
    /// </summary>
    internal static partial class Program
    {
        /// <summary>
        ///     Outputs the main ASCII banner.
        /// </summary>
        private static void ShowBanner()
        {
            ForegroundColor = Magenta;
            WriteLine(Banner);

            // outputs a string with available patterns ...
            // ... and neatly separates each pattern
            // e.g. 'nrml' | 'multi'
            var availablePatterns = new Func<string>(() =>
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
         3 - One of the following: {availablePatterns}
");
        }
    }
}