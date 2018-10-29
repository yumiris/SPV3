using System;
using System.IO;
using Atarashii.CLI.Common;
using Atarashii.CLI.Outputs;
using Atarashii.Profile;

namespace Atarashii.CLI.Commands
{
    internal partial class Profile
    {
        /// <summary>
        ///     Lastprof.txt profile name resolve sub-command.
        /// </summary>
        private static class Resolve
        {
            public static void Initialise(string[] args)
            {
                Args.ExitWhenNone(args);

                if (!File.Exists(args[0])) Exit.WithError("Given lastprof file does not exist.", 1);

                var lastprof = new Lastprof(File.ReadAllText(args[0]));
                var lastprofState = lastprof.Verify();

                if (lastprofState.IsValid)
                    Message.Show("Lastrof verification has passed.", Message.Type.Success);
                else
                    Exit.WithError(lastprofState.Reason, 2);

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
}