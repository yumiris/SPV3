using System;

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

            try
            {
                CommandFactory.Get(commands[0], new CliOutput()).Initialise(Command.FromCommand(commands));
            }
            catch (IndexOutOfRangeException)
            {
                Exit.WithError("Not enough arguments given.", 1);
            }
            catch (CommandFactoryException e)
            {
                Exit.WithError(e.Message, 2);
            }
        }
    }
}