using System;
using System.IO;
using Atarashii.CLI.Common;
using Atarashii.CLI.Outputs;
using Atarashii.Loader;

namespace Atarashii.CLI.Commands
{
    /// <summary>
    ///     CLI front-end for loading a HCE executable.
    /// </summary>
    internal class Loader : Command
    {
        public static void Initialise(string[] commands)
        {
            Exit.IfNoArgs(commands);

            var args = RemoveComFromArgs(commands);

            switch (commands[0])
            {
                case nameof(Load):
                    ShowInvokeMessage(nameof(OpenSauce), nameof(Load));
                    Load.Initialise(args);
                    break;
                case nameof(Detect):
                    ShowInvokeMessage(nameof(OpenSauce), nameof(Detect));
                    Detect.Initialise();
                    break;
                default:
                    Exit.WithError($"Invalid '{nameof(Loader)}' command given.", 1);
                    break;
            }
        }

        /// <summary>
        ///     HCE executable loading sub-command.
        /// </summary>
        private static class Load
        {
            public static void Initialise(string[] args)
            {
                Exit.IfNoArgs(args);

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

        /// <summary>
        ///     HCE executable detection sub-command.
        /// </summary>
        private static class Detect
        {
            public static void Initialise()
            {
                try
                {
                    Console.WriteLine(ExecutableFactory.Get(ExecutableFactory.Type.Detect));
                    Environment.Exit(0);
                }
                catch (FileNotFoundException e)
                {
                    Exit.WithError(e.Message, 3);
                    Environment.Exit(5);
                }
            }
        }
    }
}