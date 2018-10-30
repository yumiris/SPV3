﻿using System;
using System.IO;
using Atarashii.Loader;

namespace Atarashii.CLI.Commands
{
    /// <inheritdoc />
    /// <summary>
    ///     CLI front-end for loading a HCE executable.
    /// </summary>
    internal class Loader : Command
    {
        public Loader(Output output) : base(output)
        {
        }

        public override void Initialise(string[] commands)
        {
            ExitIfNone(commands);
            var args = GetArguments(commands);

            switch (commands[0])
            {
                case nameof(Load):
                    Load(args);
                    break;
                case nameof(Detect):
                    Detect();
                    break;
                default:
                    Fail("Invoked an invalid Load command.", 1);
                    break;
            }
        }

        private void Load(string[] args)
        {
            Info("Invoked the Loader.Load command.");
            ExitIfNone(args);

            try
            {
                var executable = new Executable(args[0], Output);
                executable.Load();
            }
            catch (LoaderException)
            {
                Environment.Exit(1);
            }
            catch (Exception)
            {
                Environment.Exit(2);
            }

            Environment.Exit(0);
        }

        private void Detect()
        {
            Info("Invoked the Loader.Detect command.");
            Info("Attempting to detect executable path.");

            try
            {
                var result = ExecutableFactory.Get(ExecutableFactory.Type.Detect, Output);
                Pass("Profile name successfully parsed:");
                Console.WriteLine(result);
                Environment.Exit(0);
            }
            catch (FileNotFoundException)
            {
                Environment.Exit(1);
            }
        }
    }
}