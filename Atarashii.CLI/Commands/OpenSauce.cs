namespace Atarashii.CLI.Commands
{
    /// <inheritdoc />
    internal partial class OpenSauce : Command
    {
        public override void Initialise(string[] commands)
        {
            Argument.ExitIfNone(commands);
            var args = Argument.FromCommand(commands);

            switch (commands[0])
            {
                case nameof(Install):
                    ShowInvokeMessage(nameof(OpenSauce), nameof(Install));
                    new Install().Initialise(args);
                    break;
                default:
                    Exit.WithError($"Invalid '{nameof(OpenSauce)}' command given.", 1);
                    break;
            }
        }
    }
}