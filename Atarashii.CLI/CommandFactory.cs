using System;

namespace Atarashii.CLI
{
    public class CommandFactory
    {
        public static Command Get(string command)
        {
            switch (command)
            {
                case nameof(Commands.Loader):
                    return new Commands.Loader();
                case nameof(Commands.OpenSauce):
                    return new Commands.OpenSauce();
                case nameof(Commands.Profile):
                    return new Commands.Profile();
                default:
                    throw new ArgumentException("Invalid command given.");
            }
        }
    }
}