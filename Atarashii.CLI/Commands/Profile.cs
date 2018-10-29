namespace Atarashii.CLI.Commands
{
    /// <inheritdoc />
    /// <summary>
    ///     CLI front-end for loading a lastprof.txt file.
    /// </summary>
    internal partial class Profile : Command
    {
        public override void Initialise(string[] commands)
        {
            Argument.ExitIfNone(commands);
            var args = Argument.FromCommand(commands);

            switch (commands[0])
            {
                case nameof(Resolve):
                    ShowInvokeMessage(nameof(Profile), nameof(Resolve));
                    new Resolve().Initialise(args);
                    break;
                default:
                    Exit.WithError($"Invalid '{nameof(Profile)}' command given.", 1);
                    break;
            }
        }
    }
}