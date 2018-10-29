using Atarashii.CLI.Common;

namespace Atarashii.CLI.Commands
{
    internal partial class OpenSauce : Command
    {
        public static void Initialise(string[] commands)
        {
            Args.ExitWhenNone(commands);
            var args = Args.FromCommand(commands);

            switch (commands[0])
            {
                case nameof(Install):
                    ShowInvokeMessage(nameof(OpenSauce), nameof(Install));
                    Install.Initialise(args);
                    break;
                default:
                    Exit.WithError($"Invalid '{nameof(OpenSauce)}' command given.", 1);
                    break;
            }
        }
    }
}