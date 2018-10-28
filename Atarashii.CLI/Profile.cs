using System;
using System.IO;
using Atarashii.Profile;

namespace Atarashii.CLI
{
    /// <summary>
    ///     CLI front-end for loading a lastprof.txt file.
    /// </summary>
    internal class Profile : Program
    {
        public static void Initiate(string[] args)
        {
            ShowBanner();
            ExitIfNilArgs(args);
            ShowMessage("Invoked parsing of " + args[0], MessageType.Info);

            if (!File.Exists(args[0]))
                ExitWithError("Given lastprof file does not exist.", 1);

            var lastprof = new Lastprof(File.ReadAllText(args[0]));
            var lastprofState = lastprof.Verify();

            if (!lastprofState.IsValid)
                ExitWithError(lastprofState.Reason, 2);

            ShowMessage("Lastrof verification has passed.", MessageType.Success);

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