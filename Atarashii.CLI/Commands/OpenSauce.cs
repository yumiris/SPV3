using System;
using Atarashii.OpenSauce;

namespace Atarashii.CLI.Commands
{
    /// <inheritdoc />
    internal class OpenSauce : Command
    {
        public OpenSauce(Output output) : base(output)
        {
        }

        public override void Initialise(string[] commands)
        {
            ExitIfNone(commands);
            var args = GetArguments(commands);

            switch (commands[0])
            {
                case nameof(Install):
                    Install(args);
                    break;
                default:
                    Fail("Invoked an invalid OpenSauce command.", 2);
                    break;
            }
        }

        private void Install(string[] args)
        {
            Info("Invoked the OpenSauce.Install command.");
            ExitIfNone(args);

            try
            {
                new InstallerFactory(args[0]).Get().Install();
            }
            catch (OpenSauceException)
            {
                Environment.Exit(1);
            }
            catch (Exception)
            {
                Environment.Exit(2);
            }
        }
    }
}