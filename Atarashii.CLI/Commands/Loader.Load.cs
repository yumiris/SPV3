using System;
using Atarashii.CLI.Common;
using Atarashii.CLI.Outputs;
using Atarashii.Loader;

namespace Atarashii.CLI.Commands
{
    internal partial class Loader
    {
        /// <summary>
        ///     HCE executable loading sub-command.
        /// </summary>
        private static class Load
        {
            public static void Initialise(string[] args)
            {
                Args.ExitWhenNone(args);

                Message.Show($"Invoked {nameof(Load)} command on '{args[0]}'.", Message.Type.Info);

                var executable = new Executable(args[0]);
                var executableState = executable.Verify();

                if (executableState.IsValid)
                    Message.Show("Executable verification has passed.", Message.Type.Success);
                else
                    Exit.WithError(executableState.Reason, 5);

                try
                {
                    executable.Load();
                    Message.Show("The specified executable has been loaded.", Message.Type.Success);
                }
                catch (LoaderException e)
                {
                    Exit.WithError(e.Message, 3);
                }
                catch (Exception e)
                {
                    Exit.WithError(e.Message, 4);
                }

                Environment.Exit(0);
            }
        }
    }
}