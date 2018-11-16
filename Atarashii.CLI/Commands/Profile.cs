using System;
using System.IO;
using Atarashii.Common;
using Atarashii.Modules.Profile;
using Atarashii.Modules.Profile.Options;

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
                Info("----------------------------------------------------------------");
                Info($"Name                         : {configuration.Name.Value}");
                Info($"Colour                       : {configuration.Colour.Value.ToString()}");
                Info("----------------------------------------------------------------");
                Info($"Mouse.Sensitivity.Horizontal : {configuration.Mouse.Sensitivity.Horizontal}");
                Info($"Mouse.Sensitivity.Vertical   : {configuration.Mouse.Sensitivity.Horizontal}");
                Info($"Mouse.InvertVerticalAxis     : {configuration.Mouse.InvertVerticalAxis}");
                Info("----------------------------------------------------------------");
                Info($"Audio.Volume.Master          : {configuration.Audio.Volume.Master}");
                Info($"Audio.Volume.Music           : {configuration.Audio.Volume.Music}");
                Info($"Audio.Volume.Effects         : {configuration.Audio.Volume.Effects}");
                Info($"Audio.Quality                : {configuration.Audio.Quality.Value}");
                Info($"Audio.Variety                : {configuration.Audio.Variety.Value}");
                Info("----------------------------------------------------------------");
                Info($"Video.Resolution.Width       : {configuration.Video.Resolution.Width}");
                Info($"Video.Resolution.Height      : {configuration.Video.Resolution.Height}");
                Info($"Video.RefreshRate            : {configuration.Video.RefreshRate.Value}");
                Info($"Video.FrameRate              : {configuration.Video.FrameRate.Value}");
                Info($"Video.Effects.Shadows        : {configuration.Video.Effects.Shadows}");
                Info($"Video.Effects.Specular       : {configuration.Video.Effects.Specular}");
                Info($"Video.Effects.Decals         : {configuration.Video.Effects.Decals}");
                Info($"Video.Particles              : {configuration.Video.Particles.Value}");
                Info($"Video.Quality (texture)      : {configuration.Video.Quality.Value}");
                Info("----------------------------------------------------------------");
                Info($"Network.Connection           : {configuration.Network.Connection.Value}");
                Info($"Network.Port.Server          : {configuration.Network.Port.Server}");
                Info($"Network.Port.Client          : {configuration.Network.Port.Client}");
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