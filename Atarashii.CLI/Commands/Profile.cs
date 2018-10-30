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
        private readonly Atarashii.Output _output;

        public Profile(Atarashii.Output output)
        {
            _output = output;
        }

        public override void Initialise(string[] commands)
        {
            Argument.ExitIfNone(commands);
            var args = Argument.FromCommand(commands);

            switch (commands[0])
            {
                case nameof(Resolve):
                    HandleInvokeType(InvokeType.Success, this, nameof(Resolve));
                    Resolve(args);
                    break;
                default:
                    HandleInvokeType(InvokeType.Invalid, this, commands[0]);
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
                _output.Write(Atarashii.Output.Type.Success, $"{nameof(Profile)}.{nameof(Resolve)}",
                    "Lastrof verification has passed.");
            else
                Exit.WithError(lastprofState.Reason, 2);

            try
            {
                var result = new Lastprof(File.ReadAllText(args[0])).Parse();
                _output.Write(Atarashii.Output.Type.Success, $"{nameof(Profile)}.{nameof(Resolve)}",
                    "Profile name successfully parsed:");
                Console.WriteLine(result);
            }
            catch (ProfileException e)
            {
                Exit.WithError(e.Message, 3);
            }
        }
    }
}