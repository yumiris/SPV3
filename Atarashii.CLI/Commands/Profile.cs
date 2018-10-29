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
        public static void Initialise(string[] commands)
        {
            Exit.IfNoArgs(commands);

            var args = RemoveComFromArgs(commands);

            switch (commands[0])
            {
                case nameof(Resolve):
                    ShowInvokeMessage(nameof(OpenSauce), nameof(Resolve));
                    Resolve.Initialise(args);
                    break;
                default:
                    Exit.WithError($"Invalid '{nameof(Profile)}' command given.", 1);
                    break;
            }
        }

        /// <summary>
        ///     Lastprof.txt profile name resolve sub-command.
        /// </summary>
        private static class Resolve
        {
            public static void Initialise(string[] args)
            {
                Exit.IfNoArgs(args);

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