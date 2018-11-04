using System;
using System.IO;
using Atarashii.Common;
using Atarashii.Modules.Profile;

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
            var args = GetArguments(commands);

            switch (commands[0])
            {
                case nameof(Resolve):
                    Resolve(args);
                    break;
                case nameof(Detect):
                    Detect();
                    break;
                default:
                    Fail("Invoked an invalid Profile command.", 2);
                    break;
            }
        }

        private void Detect()
        {
            Info("Invoked the Profile.Detect command.");

            try
            {
                var result = LastprofFactory.Get(LastprofFactory.Type.Detect, Output);
                Console.WriteLine(result.Parse());
            }
            catch (FileNotFoundException e)
            {
                Fail(e.Message, 3);
            }
            catch (ProfileException e)
            {
                Fail(e.Message, 3);
            }
        }

        private void Resolve(string[] args)
        {
            Info("Invoked the Profile.Resolve command.");
            ExitIfNone(args);

            try
            {
                Console.WriteLine(new Lastprof(File.ReadAllText(args[0])).Parse());
            }
            catch (ProfileException)
            {
                Environment.Exit(1);
            }
        }
    }
}