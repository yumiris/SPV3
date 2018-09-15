using static System.AppDomain;
using static System.Console;
using static System.ConsoleColor;
using static YuMi.Bbkpify.Ascii;

namespace YuMi.Unbbkpify.CLI
{
    internal static partial class Program
    {
        private static void ShowBanner()
        {
            ForegroundColor = Magenta;
            WriteLine(Banner);
            ForegroundColor = Cyan;
            WriteLine($@"
Usage: .\{CurrentDomain.FriendlyName} <1>
         1 - Directory name to undo the bbkpify process in (e.g. '.\cmt\tags', 'C:\cmt\tags')
");
        }
    }
}