/**
 * Copyright (C) 2019 Emilian Roman
 * 
 * This file is part of SPV3.Loader.
 * 
 * SPV3.Loader is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * SPV3.Loader is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with SPV3.Loader.  If not, see <http://www.gnu.org/licenses/>.
 */

namespace SPV3.Loader
{
    /// <summary>
    ///     Type that encompasses all HCE executable startup parameters, as specified in the doc/parameters.md
    ///     documentation.
    /// </summary>
    public class Parameters
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
    }
}