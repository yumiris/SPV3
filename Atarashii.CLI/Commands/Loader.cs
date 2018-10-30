namespace Atarashii.CLI.Commands
{
    /// <inheritdoc />
    /// <summary>
    ///     CLI front-end for loading a HCE executable.
    /// </summary>
    internal partial class Loader : Command
    {
        private readonly Atarashii.Output _output;

        public Loader(Atarashii.Output output)
        {
            _output = output;
        }

        public override void Initialise(string[] commands)
        {
            Argument.ExitIfNone(commands);
            var args = Argument.FromCommand(commands);

            switch (commands[0])
            {
                case nameof(Load):
                    HandleInvokeType(InvokeType.Success, this, nameof(Load));
                    new Load(_output).Initialise(args);
                    break;
                case nameof(Detect):
                    HandleInvokeType(InvokeType.Success, this, nameof(Detect));
                    new Detect(_output).Initialise(args);
                    break;
                default:
                    HandleInvokeType(InvokeType.Invalid, this, commands[0]);
                    break;
            }
        }
    }
}