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
                    HandleInvokeType(InvokeType.Success, this, nameof(Resolve));
                    new Resolve().Initialise(args);
                    break;
                default:
                    HandleInvokeType(InvokeType.Invalid, this, commands[0]);
                    break;
            }
        }
    }
}