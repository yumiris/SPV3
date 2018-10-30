using System;
using System.IO;
using Atarashii.Profile;

namespace Atarashii.CLI.Commands
{
    /// <inheritdoc />
    /// <summary>
    ///     CLI front-end for loading a lastprof.txt file.
    /// </summary>
    internal class Profile : Command
    {
        public Profile(Output output) : base(output)
        {
        }

        public override void Initialise(string[] commands)
        {
            ExitIfNone(commands);
            var args = FromCommand(commands);

            switch (commands[0])
            {
                case nameof(Resolve):
                    Resolve(args);
                    break;
                default:
                    Fail("Invoked an invalid Profile command.", 2);
                    break;
            }
        }

        private void Resolve(string[] args)
        {
            Info("Invoked the Profile.Resolve command.");
            ExitIfNone(args);

            if (!File.Exists(args[0])) Fail("Given lastprof file does not exist.", 1);

            var lastprof = new Lastprof(File.ReadAllText(args[0]));
            var lastprofState = lastprof.Verify();

            if (lastprofState.IsValid)
                Pass("Lastrof verification has passed.");
            else
                Fail(lastprofState.Reason, 2);

            try
            {
                var result = new Lastprof(File.ReadAllText(args[0])).Parse();
                Pass("Profile name successfully parsed:");
                Console.WriteLine(result);
            }
            catch (ProfileException e)
            {
                Fail(e.Message, 3);
            }
        }
    }
}