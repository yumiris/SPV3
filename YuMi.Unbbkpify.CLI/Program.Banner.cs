using YuMi.Bbkpify;
using static System.AppDomain;
using static System.Console;
using static System.ConsoleColor;

namespace YuMi.Unbbkpify.CLI
{
    internal static partial class Program
    {
        private static void ShowBanner()
        {
            ForegroundColor = Magenta;
            WriteLine(Ascii.Banner);
            ForegroundColor = Cyan;
            WriteLine($@"
Usage: .\{CurrentDomain.FriendlyName} <1>
         1 - Directory name to undo the bbkpify process in (e.g. '.\cmt\tags', 'C:\cmt\tags')
");
        }
    }
}