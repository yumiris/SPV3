namespace Atarashii.CLI
{
    internal class Program
    {
        /// <summary>
        ///     Main entry to the Atarashii CLI.
        /// </summary>
        /// <param name="commands">
        ///     The command to invoke.
        /// </param>
        public static void Main(string[] commands)
        {
            Banner.Show();
            Argument.ExitIfNone(commands);

            try
            {
                CommandFactory.Get(commands[0], new CliOutput()).Initialise(Argument.FromCommand(commands));
            }
            catch (CommandFactoryException e)
            {
                Exit.WithError(e.Message, 1);
            }
        }
    }
}