using System;
using System.IO;
using Atarashii.CLI;

namespace Atarashii.Profile.CLI
{
    /// <summary>
    ///     CLI front-end for loading a lastprof.txt file.
    /// </summary>
    internal class Program : BaseProgram
    {
        public static void Main(string[] args)
        {
            ShowBanner();
            ExitIfNilArgs(args);
            ShowMessage("Invoked parsing of " + args[0], MessageType.Info);

            try
            {
                var result = new Lastprof(File.ReadAllText(args[0])).Parse();
                ShowMessage("Profile name successfully parsed:", MessageType.Success);
                Console.WriteLine(result);
            }
            catch (ProfileException e)
            {
                ExitWithError(e.Message, 3);
            }
        }
    }
}