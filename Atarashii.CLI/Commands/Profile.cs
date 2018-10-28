using System;
using System.IO;
using Atarashii.CLI.Common;
using Atarashii.CLI.Outputs;
using Atarashii.Profile;

namespace Atarashii.CLI.Commands
{
    /// <summary>
    ///     CLI front-end for loading a lastprof.txt file.
    /// </summary>
    internal class Profile : Command
    {
        public static void Initiate(string[] args)
        {
            Banner.Show();
            Exit.IfNoArgs(args);
            Message.Show("Invoked parsing of " + args[0], Message.Type.Info);

            if (!File.Exists(args[0])) Exit.WithError("Given lastprof file does not exist.", 1);

            var lastprof = new Lastprof(File.ReadAllText(args[0]));
            var lastprofState = lastprof.Verify();

            if (!lastprofState.IsValid) Exit.WithError(lastprofState.Reason, 2);

            Message.Show("Lastrof verification has passed.", Message.Type.Success);

            try
            {
                var result = new Lastprof(File.ReadAllText(args[0])).Parse();
                Message.Show("Profile name successfully parsed:", Message.Type.Success);
                Console.WriteLine(result);
            }
            catch (ProfileException e)
            {
                Exit.WithError(e.Message, 3);
            }
        }
    }
}