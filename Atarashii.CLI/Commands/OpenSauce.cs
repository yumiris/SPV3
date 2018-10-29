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
                    HandleInvokeType(InvokeType.Success, this, nameof(Install));
                    new Install().Initialise(args);
                    break;
                default:
                    HandleInvokeType(InvokeType.Invalid, this, commands[0]);
                    break;
            }
        }
    }
}