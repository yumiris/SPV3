using System;
using System.IO;
using Atarashii.CLI.Common;
using Atarashii.Loader;

namespace Atarashii.CLI.Commands
{
    internal partial class Loader
    {
        /// <summary>
        ///     HCE executable detection sub-command.
        /// </summary>
        private static class Detect
        {
            public static void Initialise()
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