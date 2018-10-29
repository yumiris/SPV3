using Atarashii.CLI.Common;

namespace Atarashii.CLI.Commands
{
    /// <summary>
    ///     CLI front-end for loading a HCE executable.
    /// </summary>
    internal partial class Loader : Command
    {
        public static void Initialise(string[] commands)
        {
            Args.ExitWhenNone(commands);

            var args = Args.FromCommand(commands);

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
    }
}