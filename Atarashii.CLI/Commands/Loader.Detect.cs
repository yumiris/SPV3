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
            private readonly Atarashii.Output _output;

            public Detect(Atarashii.Output output)
            {
                _output = output;
            }

            public override void Initialise(string[] commands)
            {
                _output?.Write(Atarashii.Output.Type.Info, $"{nameof(Loader)}::{nameof(Detect)}",
                    "Attempting to detect executable path.");

                try
                {
                    var result = ExecutableFactory.Get(ExecutableFactory.Type.Detect);
                    _output?.Write(Atarashii.Output.Type.Success, $"{nameof(Loader)}::{nameof(Detect)}",
                        "Profile name successfully parsed:");
                    Console.WriteLine(result);
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