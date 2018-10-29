namespace Atarashii.CLI.Commands
{
    /// <inheritdoc />
    /// <summary>
    ///     CLI front-end for loading a HCE executable.
    /// </summary>
    internal partial class Loader : Command
    {
        public override void Initialise(string[] commands)
        {
            Argument.ExitIfNone(commands);
            var args = Argument.FromCommand(commands);

            switch (commands[0])
            {
                case nameof(Load):
                    ShowInvokeMessage(nameof(Loader), nameof(Load));
                    new Load().Initialise(args);
                    break;
                case nameof(Detect):
                    ShowInvokeMessage(nameof(Loader), nameof(Detect));
                    new Detect().Initialise(args);
                    break;
                default:
                    Exit.WithError($"Invalid '{nameof(Loader)}' command given.", 1);
                    break;
            }
        }
    }
}