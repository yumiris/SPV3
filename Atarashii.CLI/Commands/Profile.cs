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
            Argument.ExitIfNone(commands);
            var args = Argument.FromCommand(commands);

            switch (commands[0])
            {
                case nameof(Resolve):
                    OutputMessage(Output.Type.Success, "Invoked the Profile.Resolve command.");
                    Resolve(args);
                    break;
                default:
                    OutputMessage(Output.Type.Error, "Invoked an invalid Profile command.");
                    break;
            }
        }

        private void Resolve(string[] args)
        {
            Argument.ExitIfNone(args);

            if (!File.Exists(args[0])) Exit.WithError("Given lastprof file does not exist.", 1);

            var lastprof = new Lastprof(File.ReadAllText(args[0]));
            var lastprofState = lastprof.Verify();

            if (lastprofState.IsValid)
                OutputMessage(Output.Type.Success, "Lastrof verification has passed.");
            else
                Exit.WithError(lastprofState.Reason, 2);

            try
            {
                var result = new Lastprof(File.ReadAllText(args[0])).Parse();
                OutputMessage(Output.Type.Success, "Profile name successfully parsed:");
                Console.WriteLine(result);
            }
            catch (ProfileException e)
            {
                Exit.WithError(e.Message, 3);
            }
        }
    }
}