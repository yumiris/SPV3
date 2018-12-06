using System.Collections.Generic;
using System.Text;

namespace SPV3.Loader
{
    /// <summary>
    ///     Type that encompasses all HCE executable startup parameters, as specified in the doc/parameters.md
    ///     documentation.
    /// </summary>
    public class ExecutableParameters
    {
        /// <summary>
        ///     Disable all sound.
        /// </summary>
        public bool DisableSound { get; set; }

        /// <summary>
        ///     Disable video playback.
        /// </summary>
        public bool DisableVideo { get; set; }

        /// <summary>
        ///     Disable joysticks and gamepads.
        /// </summary>
        public bool DisableJoystick { get; set; }

        /// <summary>
        ///     Disables adjustment of gamma.
        /// </summary>
        public bool DisableGamma { get; set; }

        /// <summary>
        ///     The type of card HCE should be forced to run as.
        /// </summary>
        public CardType CardType { get; set; }

        /// <summary>
        ///     Disables as much as possible when running the game.
        /// </summary>
        public bool EnableSafeMode { get; set; }

        /// <summary>
        ///     Run the game in windowed mode.
        /// </summary>
        public bool EnableWindowMode { get; set; }

        /// <summary>
        ///     Enables the Print screen key to generate screenshots.
        /// </summary>
        public bool EnableScreenshot { get; set; }

        /// <summary>
        ///     Enables the debugging console.
        /// </summary>
        public bool EnableConsole { get; set; }

        /// <summary>
        ///     Enables the Halo Developer Mode.
        /// </summary>
        public bool EnableDeveloperMode { get; set; }

        /// <summary>
        ///     Forces the game to run at the width(w) specified.
        ///     Part of the video mode parameter.
        /// </summary>
        public int? VideoWidth { get; set; }

        /// <summary>
        ///     Forces the game to run at the height(h) specified.
        ///     Part of the video mode parameter.
        /// </summary>
        public int? VideoHeight { get; set; }

        /// <summary>
        ///     Forces the game to run at the refresh rate(r) specified.
        ///     Part of the video mode parameter.
        /// </summary>
        public int? VideoRefreshRate { get; set; }

        /// <summary>
        ///     Forces the game to run fullscreen on a multimon (multiple monitors) adapter.
        /// </summary>
        public int? VideoAdapterIndex { get; set; }

        /// <summary>
        ///     Server port address used when hosting multiplayer games.
        /// </summary>
        public int? ServerPort { get; set; }

        /// <summary>
        ///     Client port address used when joining multiplayer games.
        /// </summary>
        public int? ClientPort { get; set; }

        /// <summary>
        ///     Server IP address used when you have multiple IP addresses.
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        ///     Serialises the instance to a string that complies with the HCE startup arguments.
        /// </summary>
        /// <returns>
        ///     HCE-compliant startup string representation of this instance.
        /// </returns>
        public string Serialise()
        {
            var builder = new StringBuilder();

            // append the string values for toggles if they're enabled
            foreach (var toggle in new Dictionary<bool, string>
            {
                // disable overrides
                {DisableSound, "-nosound"},
                {DisableVideo, "-novideo"},
                {DisableJoystick, "-nojoystick"},
                {DisableGamma, "-nogamma"},

                // enable overrides
                {EnableSafeMode, "-safemode"},
                {EnableWindowMode, "-window"},
                {EnableScreenshot, "-screenshot"},
                {EnableConsole, "-console"},
                {EnableDeveloperMode, "-devmode"}
            })
                if (toggle.Key)
                    builder.Append($"{toggle.Value}, ");

            // shader overrides
            switch (CardType)
            {
                case CardType.FixedFunction:
                    builder.Append("-useff, ");
                    break;
                case CardType.Shaders11Card:
                    builder.Append("-use11, ");
                    break;
                case CardType.Shaders14Card:
                    builder.Append("-use14, ");
                    break;
                case CardType.Shaders20Card:
                    builder.Append("-use20, ");
                    break;
                default:
                    builder.Append(string.Empty);
                    break;
            }

            // -width
            if (VideoWidth != null)
                builder.Append($"-width {VideoWidth}, ");

            // -vidmode
            if (VideoWidth != null && VideoHeight != null && VideoRefreshRate != null)
                builder.Append($"-vidmode {VideoWidth},{VideoHeight},{VideoRefreshRate}, ");

            // -adapter
            if (VideoAdapterIndex != null)
                builder.Append($"-adapter {VideoAdapterIndex}, ");

            // -port
            if (ServerPort != null)
                builder.Append($"-port {ServerPort}, ");

            // -cport
            if (ClientPort != null)
                builder.Append($"-cport {ClientPort}, ");

            // -ip
            if (!string.IsNullOrWhiteSpace(IpAddress))
                builder.Append($"-ip {IpAddress}");

            return builder.ToString();
        }
    }
}