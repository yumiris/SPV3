using Atarashii.CLI.Common;

namespace Atarashii.CLI.Commands
{
    /// <summary>
    ///     CLI front-end for loading a lastprof.txt file.
    /// </summary>
    internal partial class Profile : Command
    {
        public static void Initialise(string[] commands)
        {
            Args.ExitWhenNone(commands);

            var args = Args.FromCommand(commands);

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
    }
}