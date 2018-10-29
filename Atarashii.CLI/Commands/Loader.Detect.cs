using System;
using System.IO;
using Atarashii.Loader;

namespace Atarashii.CLI.Commands
{
    internal partial class Loader
    {
        /// <inheritdoc />
        /// <summary>
        ///     HCE executable detection sub-command.
        /// </summary>
        private class Detect : Command
        {
            public override void Initialise(string[] commands)
            {
                try
                {
                    Console.WriteLine(ExecutableFactory.Get(ExecutableFactory.Type.Detect));
                    Environment.Exit(0);
                }
                catch (FileNotFoundException e)
                {
                    Exit.WithError(e.Message, 3);
                    Environment.Exit(5);
                }
            }
        }
    }
}