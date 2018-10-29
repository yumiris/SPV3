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
        private const string ResolveCommand = "resolve";

        public static void Initiate(string[] coms)
        {
            Exit.IfNoArgs(coms);

            var command = coms[0].ToLower();
            var args = RemoveComFromArgs(coms);

            switch (command)
            {
                case ResolveCommand:
                    HandleResolveCommand(args);
                    break;
                default:
                    Exit.WithError("Invalid arguments provided.", 2);
                    break;
            }
        }

        private static void HandleResolveCommand(string[] args)
        {
            Exit.IfNoArgs(args);
            Message.Show($"Invoked {ResolveCommand} command on '{args[0]}'.", Message.Type.Info);

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