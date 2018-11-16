using System;
using System.IO;
using Atarashii.Common;
using Atarashii.Modules.Profile;

namespace Atarashii.CLI.Commands
{
    /// <inheritdoc />
    /// <summary>
    ///     CLI front-end for loading a lastprof.txt file.
    /// </summary>
    internal class Profile : Command
    {
        public Profile(Output output) : base(output)
        {
        }

        public override void Initialise(string[] commands)
        {
            ExitIfNone(commands);
            var args = GetArguments(commands);

            switch (commands[0])
            {
                case nameof(Resolve):
                    Resolve(args);
                    break;
                case nameof(Detect):
                    Detect();
                    break;
                case nameof(Parse):
                    Parse(args);
                    break;
                default:
                    Fail("Invoked an invalid Profile command.", 2);
                    break;
            }
        }

        private void Detect()
        {
            Info("Invoked the Profile.Detect command.");

            try
            {
                var result = LastprofFactory.Get(LastprofFactory.Type.Detect, Output);
                Console.WriteLine(result.Parse());
            }
            catch (FileNotFoundException e)
            {
                Fail(e.Message, 3);
            }
            catch (ProfileException e)
            {
                Fail(e.Message, 3);
            }
        }

        private void Resolve(string[] args)
        {
            Info("Invoked the Profile.Resolve command.");
            ExitIfNone(args);

            try
            {
                Console.WriteLine(new Lastprof(File.ReadAllText(args[0])).Parse());
            }
            catch (ProfileException e)
            {
                Fail(e.Message, 3);
            }
        }

        private void Parse(string[] args)
        {
            Info("Invoked the Profile.Parse command.");
            ExitIfNone(args);

            try
            {
                var configuration = ConfigurationFactory.GetFromStream(File.Open(args[0], FileMode.Open));
                Console.WriteLine();
                Info("+ Profile ------------------------------------------------------");
                Info($"  - Name                : {configuration.Name.Value}");
                Info($"  - Colour              : {configuration.Colour.Value.ToString()}");
                Info("+ Mouse --------------------------------------------------------");
                Info("  + Sensitivity");
                Info($"    - Horizontal        : {configuration.Mouse.Sensitivity.Horizontal}");
                Info($"    - Vertical          : {configuration.Mouse.Sensitivity.Vertical}");
                Info($"  - InvertVerticalAxis  : {configuration.Mouse.InvertVerticalAxis}");
                Info("+ Audio --------------------------------------------------------");
                Info("  + Volume");
                Info($"    - Master            : {configuration.Audio.Volume.Master}");
                Info($"    - Effects           : {configuration.Audio.Volume.Effects}");
                Info($"    - Music             : {configuration.Audio.Volume.Music}");
                Info($"  - Quality             : {configuration.Audio.Quality.Value.ToString()}");
                Info($"  - Variety             : {configuration.Audio.Variety.Value.ToString()}");
                Info("+ Video --------------------------------------------------------");
                Info("  + Resolution");
                Info($"    - Width             : {configuration.Video.Resolution.Width}");
                Info($"    - Height            : {configuration.Video.Resolution.Height}");
                Info($"  - RefreshRate         : {configuration.Video.RefreshRate.Value}");
                Info($"  - FrameRate           : {configuration.Video.FrameRate.Value}");
                Info("  + Effects");
                Info($"    - Specular          : {configuration.Video.Effects.Specular}");
                Info($"    - Shadows           : {configuration.Video.Effects.Shadows}");
                Info($"    - Decals            : {configuration.Video.Effects.Decals}");
                Info($"  - Particles           : {configuration.Video.Particles.Value.ToString()}");
                Info($"  - Quality             : {configuration.Video.Quality.Value.ToString()}");
                Info("+ Network -------------------------------------------------------");
                Info($"  - Connection          : {configuration.Network.Connection.Value}");
                Info("  + Port");
                Info($"    - Server            : {configuration.Network.Port.Server}");
                Info($"    - Client            : {configuration.Network.Port.Client}");
                Console.WriteLine();
                Pass("Successfully parsed data from the provided blam.sav binary.");
            }
            catch (Exception e)
            {
                Fail(e.Message, 3);
            }
        }
    }
}