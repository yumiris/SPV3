namespace Atarashii.CLI
{
    public class CommandFactory
    {
        public static Command Get(string command, Atarashii.Output output)
        {
            switch (command)
            {
                case nameof(Commands.Loader):
                    return new Commands.Loader(output);
                case nameof(Commands.OpenSauce):
                    return new Commands.OpenSauce(output);
                case nameof(Commands.Profile):
                    return new Commands.Profile(output);
                default:
                    throw new CommandFactoryException("Invalid command given.");
            }
        }
    }
}